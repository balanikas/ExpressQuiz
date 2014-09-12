var ExpressQuiz;
(function (ExpressQuiz) {
    var ActiveQuiz = (function () {
        function ActiveQuiz(options) {
            var _this = this;
            this.options = options;
            this.onCounterUpdate = function (remainingSeconds) {
                _this.$counter.text(remainingSeconds + " seconds left");

                var timeLeftPercent = (remainingSeconds / _this.totalTime) * 100;
                _this.$counter.width(timeLeftPercent + "%");
            };
            this.onCounterEnd = function () {
                _this.sendResults();
            };
            this.onQuizFinished = function () {
                _this.counter.stop();
                _this.sendResults();
            };
            this.onAnswerSelected = function () {
                var a = $('input[name=optionsRadios]:checked', _this.$answers).val();
                _this.runtime.setAnswer(a);
                var value = _this.runtime.getProgress();
                _this.$progressBar.css('width', value + '%').attr('aria-valuenow', value);
            };
            this.onPageChanged = function (event, num) {
                var a = $('input[name=optionsRadios]:checked', _this.$answers).val();
                _this.runtime.setAnswer(a);
                _this.runtime.setActiveQuestion(num - 1);
                var question = _this.runtime.getQuestion(num - 1);
                _this.loadQuestion(question, _this.runtime.getAnswer());
            };
            this.voteCallback = function (data) {
                var vote = 0;
                if (data.upvoted) {
                    vote = 1;
                } else if (data.downvoted) {
                    vote = -1;
                }

                _this.runtime.getActiveQuestion().vote = vote;

                _this.$voting.css("pointer-events", "none");
                $.ajax({
                    url: '/Rating/RateQuestion/',
                    type: "POST",
                    cache: false,
                    data: JSON.stringify({ "id": data.id, "vote": vote }),
                    headers: ExpressQuiz.AjaxHelper.createRequestionVerificationTokenHeader(),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    complete: function () {
                        setTimeout(function () {
                            _this.$voting.css("pointer-events", "");
                        }, 1000);
                    }
                });
            };
            this.initCounter = function ($counter, totalTime) {
                _this.counter = new ExpressQuiz.CountDown({
                    seconds: totalTime,
                    onUpdateStatus: _this.onCounterUpdate,
                    onCounterEnd: _this.onCounterEnd
                });

                _this.counter.start();
            };
            this.$counter = options.$counter;
            this.$answers = options.$answers;
            this.$questions = options.$questions;
            this.$progressBar = options.$progressBar;
            this.$voting = options.$voting;
            this.$done = options.$done;
            this.$pager = options.$pager;
            this.quizId = options.quizId;
            this.totalTime = options.totalTime;

            this.init();

            ExpressQuiz.Utils.togglePreventLeavingPage(true);
        }
        ActiveQuiz.prototype.initPaging = function () {
            this.$pager.bootpag({
                total: this.runtime.quiz.Questions.length,
                page: 1,
                leaps: false,
                maxVisible: 10,
                next: "next",
                prev: "previous"
            });

            this.$pager.on("page", this.onPageChanged);

            this.loadQuestion(this.runtime.quiz.Questions[0], this.runtime.getAnswer());
        };

        ActiveQuiz.prototype.initVoting = function (question) {
            var upvote = $('<a/>').addClass('upvote');
            var downvote = $('<a/>').addClass('downvote');
            var container = $('<div />').append(upvote).append(downvote).addClass('upvote-superuser').addClass('upvote');
            this.$voting.empty().append(container);

            var upVoted = question.vote == 1;
            var downVoted = question.vote == -1;
            container.upvote({ id: question.QuestionId, callback: this.voteCallback, upvoted: upVoted, downvoted: downVoted });
        };

        ActiveQuiz.prototype.loadQuestion = function (question, answerId) {
            this.$answers.empty();

            for (var i = 0; i < question.Answers.length; i++) {
                var checked = answerId == question.Answers[i].AnswerId ? "checked" : "";

                var div = $(" <div class='radio'/>");
                var label = $("<label/>");
                var input = $("<input type='radio' " + checked + " name='optionsRadios' id=" + i + " value=" + i + ">");

                label.append(input);
                label.append(question.Answers[i].Text);

                div.append(label);
                this.$answers.append(div);
            }

            $('input[type=radio][name=optionsRadios]').on('change', this.onAnswerSelected);

            this.$questions.empty().append($("<textarea class='pagedown hidden' >" + question.Text + "</textarea>"));

            $("textarea.pagedown").pagedownBootstrap({});
            $('.wmd-preview').addClass('well');
            $(".wmd-button-bar").hide();

            this.initVoting(question);
        };

        ActiveQuiz.prototype.init = function () {
            var _this = this;
            $.ajax({
                url: "/ActiveQuiz/GetQuiz/" + this.quizId,
                type: "GET",
                error: function (jqXHR, textStatus, errorThrown) {
                    ExpressQuiz.Utils.togglePreventLeavingPage(false);
                    location.href = "/Home/Error/?message=" + errorThrown;
                },
                success: function (data) {
                    _this.runtime = new ExpressQuiz.Runtime(data);
                    _this.initPaging();
                    _this.initCounter(_this.$counter, _this.totalTime);
                    _this.$done.click(_this.onQuizFinished);
                    _this.counter.start();
                }
            });
        };

        ActiveQuiz.prototype.sendResults = function () {
            var _this = this;
            this.$done.addClass('active');
            this.$counter.removeClass("active");

            var result;
            result = {
                QuizId: this.quizId,
                UserAnswers: this.runtime.getResult(),
                EllapsedTime: this.counter.getRemainingTime()
            };

            $.ajax({
                url: '/ActiveQuiz/PostResult/',
                type: "POST",
                cache: false,
                headers: ExpressQuiz.AjaxHelper.createRequestionVerificationTokenHeader(),
                data: JSON.stringify(result),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                error: function (jqXHR, textStatus, errorThrown) {
                    ExpressQuiz.Utils.togglePreventLeavingPage(false);
                    location.href = "/Home/Error/?message=" + errorThrown;
                },
                success: function (data, textStatus, jqXHR) {
                    var url = '/QuizReview/Index/' + data;
                    ExpressQuiz.Utils.togglePreventLeavingPage(false);
                    window.location.href = url;
                },
                complete: function () {
                    _this.$done.addClass('active');
                }
            });
        };
        return ActiveQuiz;
    })();
    ExpressQuiz.ActiveQuiz = ActiveQuiz;
})(ExpressQuiz || (ExpressQuiz = {}));
//# sourceMappingURL=activequiz.js.map
