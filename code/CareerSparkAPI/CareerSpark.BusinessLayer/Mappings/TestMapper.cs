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

        public static CareerRoadmapDto ToCareerRoadmapDto(CareerRoadmap roadmap)
        {
            return new CareerRoadmapDto
            {
                Id = roadmap.Id,
                StepOrder = roadmap.StepOrder,
                Title = roadmap.Title,
                Description = roadmap.Description,
                SkillFocus = roadmap.SkillFocus,
                DifficultyLevel = roadmap.DifficultyLevel,
                DurationWeeks = roadmap.DurationWeeks,
                SuggestedCourseUrl = roadmap.SuggestedCourseUrl
            };
        }

        public static CareerPathDto ToCareerPathDto(CareerPath path, List<CareerRoadmapDto> roadmaps)
        {
            return new CareerPathDto
            {
                Id = path.Id,
                Title = path.Title,
                Description = path.Description,
                CareerFieldId = path.CareerFieldId,
                Roadmaps = roadmaps
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
