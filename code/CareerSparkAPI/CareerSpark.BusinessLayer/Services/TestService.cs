using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.BusinessLayer.Interfaces;
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

            var session = new TestSession
            {
                UserId = request.UserId,
                StartAt = DateTime.Now
            };

            _uow.TestSessionRepository.PrepareCreate(session);
            await _uow.SaveAsync();

            _logger.LogInformation("Created TestSession Id={SessionId}, UserId={UserId}", session.Id, session.UserId);

            return new StartTestResponse
            {
                SessionId = session.Id,
                StartAt = session.StartAt
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
                        Score = g.Count(x => x.IsSelected),
                        Total = g.Count(),
                        Normalized = (g.Count(x => x.IsSelected) * 100.0) / g.Count()
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

                // 4. Trả response
                return new TestResultResponse
                {
                    R = result.R,
                    I = result.I,
                    A = result.A,
                    S = result.S,
                    E = result.E,
                    C = result.C,

                    R_Normalized = GetNorm("Realistic"),
                    I_Normalized = GetNorm("Investigative"),
                    A_Normalized = GetNorm("Artistic"),
                    S_Normalized = GetNorm("Social"),
                    E_Normalized = GetNorm("Enterprising"),
                    C_Normalized = GetNorm("Conventional")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SubmitTestAsync for SessionId={SessionId}", request.SessionId);
                await _uow.RollbackTransactionAsync();
                throw;
            }
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

            // 2. Lấy kết quả RIASEC từ TestSession
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

            // 3. Mapping CareerField (ví dụ logic demo)
            int careerField;
            if (result.I >= result.R && result.I >= result.S)
                careerField = 1;
            else
                careerField = 2;
            _logger.LogInformation("Mapped CareerField={CareerField}", careerField);

            // 4. Lấy CareerPath + Milestones
            var paths = await _uow.CareerPathRepository.GetAllWithCareerFieldAsync();
            var milestones = await _uow.CareerMilestoneRepository.GetAllAsync();

            _logger.LogInformation("Loaded {PathCount} career paths, {MilestoneCount} milestones",
       paths.Count, milestones.Count);

            var filteredPaths = paths
                .Where(p => p.CareerField.Id == careerField)
                .Select(p => new CareerPathDto
                {
                    Title = p.Title,
                    Description = p.Description,
                    Milestones = milestones
                        .Where(m => m.CareerPathId == p.Id)
                        .Select(m => new CareerMilestoneDto
                        {
                            Title = m.Title,
                            Description = m.Description,
                            SuggestedCourseUrl = m.SuggestedCourseUrl
                        }).ToList()
                }).ToList();

            _logger.LogInformation("Filtered {FilteredCount} career paths for CareerField={CareerField}",
       filteredPaths.Count, careerField);

            return new CareerPathResponse
            {
                CareerField = careerField.ToString(),
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
                      (ta, q) => new TestHistoryAnswerDto
                      {
                          QuestionId = q.Id,
                          QuestionContent = q.Content,
                          QuestionType = q.QuestionType,
                          IsSelected = ta.IsSelected
                      }).ToList();

            return new TestHistoryResponse
            {
                SessionId = session.Id,
                UserId = session.UserId,
                StartAt = session.StartAt,
                EndAt = session.EndAt,
                Answers = joined
            };
        }


    }
}
