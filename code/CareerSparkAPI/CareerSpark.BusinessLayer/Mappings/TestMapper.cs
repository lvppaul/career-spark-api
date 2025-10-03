using CareerSpark.BusinessLayer.DTOs.Request;
using CareerSpark.BusinessLayer.DTOs.Response;
using CareerSpark.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CareerSpark.BusinessLayer.Mappings
{
    public static class TestMapper
    {
        public static CareerFieldDto ToCareerFieldDto(CareerField entity)
        {
            return new CareerFieldDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public static CareerMilestoneDto ToCareerMilestoneDto(CareerMilestone entity)
        {
            return new CareerMilestoneDto
            {
                Title = entity.Title,
                Description = entity.Description ?? string.Empty,
                SuggestedCourseUrl = entity.SuggestedCourseUrl ?? string.Empty
            };
        }

        public static CareerPathDto ToCareerPathDto(CareerPath path, IEnumerable<CareerMilestone> allMilestones)
        {
            var milestones = allMilestones
                .Where(m => m.CareerPathId == path.Id)
                .Select(ToCareerMilestoneDto)
                .ToList();

            return new CareerPathDto
            {
                Title = path.Title,
                Description = path.Description ?? string.Empty,
                Milestones = milestones
            };
        }

        public static TestHistoryAnswerDto ToTestHistoryAnswerDto(QuestionTest question, TestAnswer answer)
        {
            return new TestHistoryAnswerDto
            {
                QuestionId = question.Id,
                QuestionContent = question.Content,
                QuestionType = question.QuestionType,
                IsSelected = answer.IsSelected ?? false
            };
        }

        public static QuestionTestResponse ToQuestionTestResponse(QuestionTest question)
        {
            return new QuestionTestResponse
            {
                Id = question.Id,
                Content = question.Content,
                QuestionType = question.QuestionType
            };
        }
    }
}
