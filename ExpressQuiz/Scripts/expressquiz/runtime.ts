


module ExpressQuiz {
    "use strict";

    export class Runtime {
        private userAnswers: Array<UserAnswer>;
        private currentQuestionIndex: number;

        constructor(public quiz: any) {
            this.userAnswers = [];
            this.currentQuestionIndex = 0;
            for (var i = 0; i < quiz.Questions.length; i++) {
                this.userAnswers[i] = new UserAnswer(quiz.Questions[i].QuestionId, undefined);
            }
        }


        setActiveQuestion(index: number): void {

            this.currentQuestionIndex = index;

        }

        getActiveQuestion(): any {
            return this.quiz.Questions[this.currentQuestionIndex];
        }

        getQuestion(index: number): any {
            return this.quiz.Questions[index];
        }

        setAnswer(answer: number): void {

            var q = this.quiz.Questions[this.currentQuestionIndex];
            var userAnswer;
            if (answer === undefined) {
                userAnswer = new UserAnswer(q.QuestionId, undefined);
            } else {
                var a = q.Answers[answer];
                userAnswer = new UserAnswer(q.QuestionId, a.AnswerId);
            }

            this.userAnswers[this.currentQuestionIndex] = userAnswer;
        }

        getAnswer(): number {

            return this.userAnswers[this.currentQuestionIndex].answerId;
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