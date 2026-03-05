using EducationalContentGeneration.Core.Models;

namespace EducationalContentGeneration.UI.Components.Services
{
    public class MockDataService
    {
        public List<McqQuestionResponse> GenerateMcq(ContentRequest request)
        {
            return new List<McqQuestionResponse>
               {
                   new McqQuestionResponse
                   {
                       Number = 1,
                       Question = "What is the smallest unit of matter?",
                       Options = new List<string>
                       {
                           "Atom",
                           "Molecule",
                           "Cell",
                           "Electron"
                       },
                       CorrectAnswer = "Atom",
                       Explanation = "An atom is the smallest unit of matter that retains the properties of an element."
                   },

                   new McqQuestionResponse
                   {
                       Number = 2,
                       Question = "Which particle has a negative charge?",
                       Options = new List<string>
                       {
                           "Proton",
                           "Electron",
                           "Neutron",
                           "Nucleus"
                       },
                       CorrectAnswer = "Electron",
                       Explanation = "Electrons are negatively charged particles found outside the nucleus."
                   }
               };
        }

        public string GenerateShortAnswer(ContentRequest request)
        {
            return $@" SHORT ANSWER GENERATED SUCCESSFULLY

                  Subject: {request.Subject}
                  Topic: {request.Topic}
                  Class: {request.ClassLevel}
                  Difficulty: {request.Difficulty}
                  Count: {request.NumberOfQuestions}

                  1. Sample Question?
                  Answer:
                  This is a sample short answer explaining the concept briefly

                  Key Points:
                  - Key point one
                  - Key point two 
                  - Key point three
                 ";
        }

        public string GenerateLongAnswer(ContentRequest request)
        {
            return $@" LONG ANSWER GENERATED SUCCESSFULLY

              Subject: {request.Subject}
              Topic: {request.Topic}
              Class: {request.ClassLevel}
              Difficulty: {request.Difficulty}
              Count: {request.NumberOfQuestions}

              1. Sample Question?
              Answer:
              This is a detailed long answer explaining the concept in depth.
              It includes background, explanation, and examples where required 

              Key Points:
              - Detailed explanation of the core concept
              - Supporting examples
              - Summary conclusion
             ";
        }

        public string GenerateExplanation(ExplanationRequest request)
        {
            return $@" EXPLANATION GENERATED SUCCESSFULLY

              Subject:{request.Subject}
              Topic:{request.Topic}
              Class:{request.ClassLevel}
              Difficulty:{request.Difficulty}


               Question:
               {request.Questions[0].Question}

               Answer:
               {request.Questions[0].Answer}

               Detailed Explanation
               The above answer is correct because
               {request.Topic} involves key concepts that students at {request.ClassLevel} must understand.
               This explanation expands on why the answer works and clarifies common misconceptions.

              Key Points:
              Core concept clarification
              step-by-step reasoning
              Practical understanding
             ";
        }
    }
}
