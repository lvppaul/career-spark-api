using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.Interfaces;
using CareerSpark.BusinessLayer.Mappings;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerSpark.BusinessLayer.Services
{
    public class TestService : ITestService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<TestService> _logger;

        public TestService(IUnitOfWork uow, ILogger<TestService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<StartTestResponse> StartTestAsync(StartTestRequest request)
        {
            _logger.LogInformation("StartTestAsync called for UserId={UserId}", request.UserId);

            var now = DateTime.Now;
            var session = new TestSession
            {
                UserId = request.UserId,
                StartAt = now
            };

            _uow.TestSessionRepository.PrepareCreate(session);
            await _uow.SaveAsync();

            _logger.LogInformation("Created TestSession Id={SessionId}, UserId={UserId}", session.Id, session.UserId);

            return new StartTestResponse
            {
                SessionId = session.Id,
                StartAt = now
            };
        }

        public async Task<TestResultResponse> SubmitTestAsync(SubmitTestRequest request)
        {

            _logger.LogInformation("SubmitTestAsync called with SessionId={SessionId}, AnswerCount={Count}",
       request.SessionId, request.Answers?.Count);
            await _uow.BeginTransactionAsync();

            try
            {
                // 1. Lưu câu trả lời
                foreach (var ans in request.Answers)
                {
                    _logger.LogDebug("Answer: QuestionId={Q}, IsSelected={S}", ans.QuestionId, ans.IsSelected);
                    var testAnswer = new TestAnswer
                    {
                        TestSessionId = request.SessionId,
                        QuestionId = ans.QuestionId,
                        IsSelected = ans.IsSelected
                    };
                    _uow.TestAnswerRepository.PrepareCreate(testAnswer);
                    await _uow.SaveAsync();


                    // 2. Log lại vào TestHistory
                    var testHistory = new TestHistory
                    {
                        UserId = request.UserId,
                        TestSessionId = request.SessionId,
                        TestAnswerId = testAnswer.Id
                    };
                    _uow.TestHistoryRepository.PrepareCreate(testHistory);
                    await _uow.SaveAsync();
                }


                // 2. Lấy dữ liệu Answer + Question để tính điểm
                var answers = await _uow.TestAnswerRepository.GetAllAsync();
                var questions = await _uow.QuestionTestRepository.GetAllAsync();

                _logger.LogInformation("Loaded {AnswerCount} answers, {QuestionCount} questions",
                    answers.Count, questions.Count);

                var joined = answers
                    .Where(a => a.TestSessionId == request.SessionId)
                    .Join(questions,
                          ta => ta.QuestionId,
                          q => q.Id,
                          (ta, q) => new { ta.IsSelected, q.QuestionType });

                var scores = joined
                    .GroupBy(x => x.QuestionType)
                    .Select(g => new
                    {
                        Type = g.Key,
                        Score = g.Count(x => x.IsSelected == true),
                        Total = g.Count(),
                        Normalized = (g.Count(x => x.IsSelected == true) * 100.0) / g.Count()
                    }).ToList();
                foreach (var s in scores)
                    _logger.LogInformation("Score {Type}: {Score}/{Total} ({Norm}%)", s.Type, s.Score, s.Total, s.Normalized);

                int GetScore(string type) => scores.FirstOrDefault(x => x.Type == type)?.Score ?? 0;
                double GetNorm(string type) => scores.FirstOrDefault(x => x.Type == type)?.Normalized ?? 0;

                // 3. Lưu Result
                var result = new Result
                {
                    TestSessionId = request.SessionId,
                    R = GetScore("Realistic"),
                    I = GetScore("Investigative"),
                    A = GetScore("Artistic"),
                    S = GetScore("Social"),
                    E = GetScore("Enterprising"),
                    C = GetScore("Conventional"),
                    Content = "RIASEC Test Result"
                };

                _uow.ResultRepository.PrepareCreate(result);
                await _uow.SaveAsync();

                await _uow.CommitTransactionAsync();

                // 4. Ánh xạ CareerField qua CareerMapping
                string topType = GetTopRiasecType(result);
                var mappings = await _uow.CareerMappingRepository.GetAllWithFieldAsync();
                var suggestedFields = mappings
                    .Where(m => m.RiasecType == topType)
                    .Select(m => TestMapper.ToCareerFieldDto(m.CareerField))
                    .ToList();

                // 5. Trả response
                return new TestResultResponse
                {
                    R = result.R ?? 0,
                    I = result.I ?? 0,
                    A = result.A ?? 0,
                    S = result.S ?? 0,
                    E = result.E ?? 0,
                    C = result.C ?? 0,

                    R_Normalized = GetNorm("Realistic"),
                    I_Normalized = GetNorm("Investigative"),
                    A_Normalized = GetNorm("Artistic"),
                    S_Normalized = GetNorm("Social"),
                    E_Normalized = GetNorm("Enterprising"),
                    C_Normalized = GetNorm("Conventional"),

                    SuggestedCareerFields = suggestedFields
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SubmitTestAsync for SessionId={SessionId}", request.SessionId);
                await _uow.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<TestResultResponse> GetResultBySessionAsync(int sessionId)
        {
            var results = await _uow.ResultRepository.GetAllAsync();
            var result = results.FirstOrDefault(r => r.TestSessionId == sessionId);
            if (result == null)
            {
                throw new Exception("Result not found");
            }

            // Compute normalized values by looking up questions of each type
            var answers = await _uow.TestAnswerRepository.GetAllAsync();
            var questions = await _uow.QuestionTestRepository.GetAllAsync();
            var joined = answers
                .Where(a => a.TestSessionId == sessionId)
                .Join(questions,
                      ta => ta.QuestionId,
                      q => q.Id,
                      (ta, q) => new { ta.IsSelected, q.QuestionType });

            double Norm(string type)
            {
                var group = joined.Where(x => x.QuestionType == type).ToList();
                if (group.Count == 0) return 0;
                var selected = group.Count(x => x.IsSelected == true);
                return selected * 100.0 / group.Count;
            }

            // Suggested fields by top type
            string topType = GetTopRiasecType(result);
            var mappings = await _uow.CareerMappingRepository.GetAllWithFieldAsync();
            var suggestedFields = mappings
                .Where(m => m.RiasecType == topType)
                .Select(m => TestMapper.ToCareerFieldDto(m.CareerField))
                .ToList();

            return new TestResultResponse
            {
                R = result.R ?? 0,
                I = result.I ?? 0,
                A = result.A ?? 0,
                S = result.S ?? 0,
                E = result.E ?? 0,
                C = result.C ?? 0,
                R_Normalized = Norm("Realistic"),
                I_Normalized = Norm("Investigative"),
                A_Normalized = Norm("Artistic"),
                S_Normalized = Norm("Social"),
                E_Normalized = Norm("Enterprising"),
                C_Normalized = Norm("Conventional"),
                SuggestedCareerFields = suggestedFields
            };
        }


        public async Task<CareerPathResponse> GetRoadmapAsync(int sessionId, int userId)
        {
            _logger.LogInformation("GetRoadmapAsync called with SessionId={SessionId}, UserId={UserId}", sessionId, userId);
            //// 1. Kiểm tra subscription
            //var subs = await _uow.UserSubscriptionRepository.GetAllAsync();
            //var hasActiveSub = subs.Any(us =>
            // us.UserId == userId &&
            //(us.IsActive ?? false) &&
            // us.EndDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Today);



            //if (!hasActiveSub)
            //    throw new UnauthorizedAccessException("Bạn cần mua gói thành viên để xem roadmap");

            // 2. Lấy kết quả RIASEC từ TestSession và mapping career field
            var results = await _uow.ResultRepository.GetAllAsync();
            _logger.LogInformation("Loaded {Count} results from DB", results?.Count);
            var result = results.FirstOrDefault(r => r.TestSessionId == sessionId);
            if (result == null)
            {
                _logger.LogWarning("No result found for SessionId={SessionId}", sessionId);
                throw new Exception("Result not found");
            }
            _logger.LogInformation("Result found: R={R}, I={I}, A={A}, S={S}, E={E}, C={C}",
       result.R, result.I, result.A, result.S, result.E, result.C);



            string topType = GetTopRiasecType(result);
            var mappings = await _uow.CareerMappingRepository.GetAllAsync();
            var careerFieldId = mappings.FirstOrDefault(m => m.RiasecType == topType)?.CareerFieldId;

            if (careerFieldId == null)
                throw new Exception($"No CareerField mapping found for type {topType}");


            // 4. Lấy CareerPath + Milestones
            var paths = await _uow.CareerPathRepository.GetAllWithCareerFieldAsync();
            var roadmaps = await _uow.CareerRoadmapRepository.GetAllAsync();

            _logger.LogInformation("Loaded {PathCount} career paths, {MilestoneCount} milestones",
       paths.Count, roadmaps.Count);

            var filteredPaths = paths
            .Where(p => p.CareerField.Id == careerFieldId)
            .Select(p =>
            {
              var roadmapDtos = roadmaps
                .Where(r => r.CareerPathId == p.Id)
                .OrderBy(r => r.StepOrder)
                .Select(TestMapper.ToCareerRoadmapDto) 
                .ToList();

                    return TestMapper.ToCareerPathDto(p, roadmapDtos);
                })
                  .ToList();

            var field = await _uow.CareerFieldRepository.GetByIdAsync(careerFieldId.Value);

            _logger.LogInformation("Filtered {FilteredCount} career paths for CareerField={CareerField}",
       filteredPaths.Count, careerFieldId);

            return new CareerPathResponse
            {
                CareerField = TestMapper.ToCareerFieldDto(field),
                Paths = filteredPaths
            };
        }

        public async Task<TestHistoryResponse> GetTestHistoryAsync(int sessionId, int userId)
        {
            // Lấy TestSession
            var session = await _uow.TestSessionRepository.GetByIdAsync(sessionId);
            if (session == null || session.UserId != userId)
                throw new Exception("Session not found or not belong to user");

            // Lấy TestAnswer + Question
            var answers = await _uow.TestAnswerRepository.GetAllAsync();
            var questions = await _uow.QuestionTestRepository.GetAllAsync();

            var joined = answers
                .Where(a => a.TestSessionId == sessionId)
                .Join(questions,
                      ta => ta.QuestionId,
                      q => q.Id,
                      (ta, q) => TestMapper.ToTestHistoryAnswerDto(q, ta))
                      .ToList();

            return new TestHistoryResponse
            {
                SessionId = session.Id,
                UserId = session.UserId,
                StartAt = session.StartAt ?? DateTime.MinValue,
                Answers = joined
            };
        }

        public async Task<List<TestSessionDto>> GetUserTestSessionsAsync(int userId)
        {
            _logger.LogInformation("GetUserTestSessionsAsync called for UserId={UserId}", userId);
            var sessions = await _uow.TestSessionRepository.GetAllAsync();
            var userSessions = sessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.StartAt)
                .Select(s => new TestSessionDto
                {
                    SessionId = s.Id,
                    StartAt = s.StartAt ?? DateTime.MinValue
                })
                .ToList();

            _logger.LogInformation("Found {Count} sessions for UserId={UserId}", userSessions.Count, userId);
            return userSessions;
        }

        // Helper lấy RIASEC cao nhất
        private string GetTopRiasecType(Result result)
        {
            var dict = new Dictionary<string, int>
            {
                { "R", result.R ?? 0 },
                { "I", result.I ?? 0 },
                { "A", result.A ?? 0 },
                { "S", result.S ?? 0 },
                { "E", result.E ?? 0 },
                { "C", result.C ?? 0 }
            };

            return dict.OrderByDescending(x => x.Value).First().Key;
        }

        public async Task<List<QuestionTestResponse>> GetQuestionsAsync()
        {
            _logger.LogInformation("GetQuestionsAsync called");
            var questions = await _uow.QuestionTestRepository.GetAllAsync();
            var result = questions.Select(TestMapper.ToQuestionTestResponse).ToList();
            _logger.LogInformation("GetQuestionsAsync returning {Count} items", result.Count);
            return result;
        }

        // New: get the latest session of a user
        public async Task<TestSessionDto?> GetLatestUserTestSessionAsync(int userId)
        {
            _logger.LogInformation("GetLatestUserTestSessionAsync called for UserId={UserId}", userId);
            var sessions = await _uow.TestSessionRepository.GetAllAsync();
            var latest = sessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.StartAt)
                .FirstOrDefault();

            if (latest == null)
            {
                _logger.LogInformation("No sessions found for UserId={UserId}", userId);
                return null;
            }

            return new TestSessionDto
            {
                SessionId = latest.Id,
                StartAt = latest.StartAt ?? DateTime.MinValue
            };
        }
    }
}
