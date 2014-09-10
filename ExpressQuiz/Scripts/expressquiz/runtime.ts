


module ExpressQuiz {

    export class Runtime {
        private userAnswers: Array<UserAnswer>
        private currentQuestionIndex: number;

        constructor(public quiz: any) {
            this.userAnswers = [];
            this.currentQuestionIndex = 0;
            for (var i = 0; i < quiz.Questions.length; i++) {
                this.userAnswers[i] = new UserAnswer(quiz.Questions[i].QuestionId, undefined);
            }
        }



        setActiveQuestion(index: number) {


            //if (this.quiz.Questions[index] === undefined) {
            //    throw "index out of range";
            //}
            this.currentQuestionIndex = index;
            // return this.quiz.Questions[index];
        }

        getActiveQuestion(index: number) {
            if (index === undefined) {
                return this.quiz.Questions[this.currentQuestionIndex];
            } else {
                return this.quiz.Questions[index];
            }
            
        }

        setAnswer(index: number, answer: number) {

            var q = this.quiz.Questions[index];
            var userAnswer;
            if (answer === undefined) {
                userAnswer = new UserAnswer(q.QuestionId, undefined);
            } else {
                var a = q.Answers[answer];
                userAnswer = new UserAnswer(q.QuestionId, a.AnswerId);
            }

            this.userAnswers[index] = userAnswer;
        }

        getAnswer(index: number) {

            return this.userAnswers[index].answerId;
        }

        getProgress(): number {
            var qCount = this.quiz.Questions.length;
            var answered = 0;
            for (var i = 0; i < qCount; i++) {
                if (this.userAnswers[i].answerId !== undefined) {
                    answered++;
                }
            }

            return (answered / qCount) * 100;
        }

        getResult(): any {
            return this.userAnswers;
        }
    }
}

 