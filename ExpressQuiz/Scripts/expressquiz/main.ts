


module ExpressQuiz {

    export class UserAnswer {
        constructor(public AnswerId: number, public QuestionId: number) {
            
        }
    }
    export class Answer {


        constructor(public id: number, public text: string, public isCorrect: boolean) {

        }
    }


    export class Question {
        constructor(public id: number, public text: string, public answers: Array<Answer>) {
        }
    }


    export class Quiz {
        constructor(public id: number, public desc: string, public questions: Array<Question>) {
        }
    }

    export class Runtime {
        private userAnswers: Array<UserAnswer>
        private currentQuestionIndex: number;
        
        constructor(public quiz: any) {
            this.userAnswers = [];
            this.currentQuestionIndex = 0;
            for (var i = 0; i < quiz.Questions.length; i++) {
                this.userAnswers[i] = undefined;
            }
        }



        setActiveQuestion(index: number) {

            
            if (this.quiz.Questions[index] === undefined) {
                throw "index out of range";
            }
            this.currentQuestionIndex = index;
            return this.quiz.Questions[index];
        }

        getActiveQuestion(index: number) {
          
            return this.quiz.Questions[index];
        }

        setAnswer(index: number, answer: number) {
            var q = this.quiz.Questions[index];
            var a = q.Answers[answer];
            var userAnswer = new UserAnswer(a.ID, q.ID);
            this.userAnswers[index] = userAnswer;
        }

        getAnswer(index: number) {
            return this.userAnswers[index];
        }

        getProgress(): number {
            var qCount = this.quiz.Questions.length;
            var answered = 0;
            for (var i = 0; i < qCount; i++) {
                if (this.userAnswers[i] !== undefined) {
                    answered++;
                }
            }

            return (answered / qCount)*100;
        }

        getResult(): any {
            return this.userAnswers;
        }
    }
}

