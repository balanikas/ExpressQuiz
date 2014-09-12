var ExpressQuiz;
(function (ExpressQuiz) {
    var Runtime = (function () {
        function Runtime(quiz) {
            this.quiz = quiz;
            this.userAnswers = [];
            this.currentQuestionIndex = 0;
            for (var i = 0; i < quiz.Questions.length; i++) {
                this.userAnswers[i] = new ExpressQuiz.UserAnswer(quiz.Questions[i].QuestionId, undefined);
            }
        }
        Runtime.prototype.setActiveQuestion = function (index) {
            //if (this.quiz.Questions[index] === undefined) {
            //    throw "index out of range";
            //}
            this.currentQuestionIndex = index;
            // return this.quiz.Questions[index];
        };

        Runtime.prototype.getActiveQuestion = function () {
            return this.quiz.Questions[this.currentQuestionIndex];
        };

        Runtime.prototype.getQuestion = function (index) {
            return this.quiz.Questions[index];
        };

        Runtime.prototype.setAnswer = function (answer) {
            var q = this.quiz.Questions[this.currentQuestionIndex];
            var userAnswer;
            if (answer === undefined) {
                userAnswer = new ExpressQuiz.UserAnswer(q.QuestionId, undefined);
            } else {
                var a = q.Answers[answer];
                userAnswer = new ExpressQuiz.UserAnswer(q.QuestionId, a.AnswerId);
            }

            this.userAnswers[this.currentQuestionIndex] = userAnswer;
        };

        Runtime.prototype.getAnswer = function () {
            return this.userAnswers[this.currentQuestionIndex].answerId;
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
//# sourceMappingURL=runtime.js.map
