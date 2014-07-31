var ExpressQuiz;
(function (ExpressQuiz) {
    var UserAnswer = (function () {
        function UserAnswer(questionId, answerId) {
            this.questionId = questionId;
            this.answerId = answerId;
        }
        return UserAnswer;
    })();
    ExpressQuiz.UserAnswer = UserAnswer;
    var Answer = (function () {
        function Answer(id, text, isCorrect) {
            this.id = id;
            this.text = text;
            this.isCorrect = isCorrect;
        }
        return Answer;
    })();
    ExpressQuiz.Answer = Answer;

    var Question = (function () {
        function Question(id, text, answers) {
            this.id = id;
            this.text = text;
            this.answers = answers;
        }
        return Question;
    })();
    ExpressQuiz.Question = Question;

    var Quiz = (function () {
        function Quiz(id, desc, questions) {
            this.id = id;
            this.desc = desc;
            this.questions = questions;
        }
        return Quiz;
    })();
    ExpressQuiz.Quiz = Quiz;

    var Runtime = (function () {
        function Runtime(quiz) {
            this.quiz = quiz;
            this.userAnswers = [];
            this.currentQuestionIndex = 0;
            for (var i = 0; i < quiz.Questions.length; i++) {
                this.userAnswers[i] = new UserAnswer(quiz.Questions[i].Id, undefined);
            }
        }
        Runtime.prototype.setActiveQuestion = function (index) {
            //if (this.quiz.Questions[index] === undefined) {
            //    throw "index out of range";
            //}
            this.currentQuestionIndex = index;
            // return this.quiz.Questions[index];
        };

        Runtime.prototype.getActiveQuestion = function (index) {
            return this.quiz.Questions[index];
        };

        Runtime.prototype.setAnswer = function (index, answer) {
            var q = this.quiz.Questions[index];
            var userAnswer;
            if (answer === undefined) {
                userAnswer = new UserAnswer(q.Id, undefined);
            } else {
                var a = q.Answers[answer];
                userAnswer = new UserAnswer(q.Id, a.Id);
            }

            this.userAnswers[index] = userAnswer;
        };

        Runtime.prototype.getAnswer = function (index) {
            return this.userAnswers[index].answerId;
        };

        Runtime.prototype.getProgress = function () {
            var qCount = this.quiz.Questions.length;
            var answered = 0;
            for (var i = 0; i < qCount; i++) {
                if (this.userAnswers[i].answerId !== undefined) {
                    answered++;
                }
            }

            return (answered / qCount) * 100;
        };

        Runtime.prototype.getResult = function () {
            return this.userAnswers;
        };
        return Runtime;
    })();
    ExpressQuiz.Runtime = Runtime;
})(ExpressQuiz || (ExpressQuiz = {}));
//# sourceMappingURL=main.js.map
