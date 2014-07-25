var ExpressQuiz;
(function (ExpressQuiz) {
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
            this.userAnswers = {};
            for (var i = 0; i < quiz.questions.length; i++) {
                this.userAnswers[i] = undefined;
            }
        }
        Runtime.prototype.setActiveQuestion = function (index) {
            if (this.quiz.questions[index] === undefined) {
                throw "index out of range";
            }
            this.currentQuestionIndex = index;
            return this.quiz.questions[index];
        };

        Runtime.prototype.getActiveQuestion = function (index) {
            return this.quiz.questions[index];
        };

        Runtime.prototype.setAnswer = function (index, answer) {
            this.userAnswers[index] = answer;
        };

        Runtime.prototype.getAnswer = function (index) {
            return this.userAnswers[index];
        };
        return Runtime;
    })();
    ExpressQuiz.Runtime = Runtime;
})(ExpressQuiz || (ExpressQuiz = {}));
//# sourceMappingURL=test.js.map
