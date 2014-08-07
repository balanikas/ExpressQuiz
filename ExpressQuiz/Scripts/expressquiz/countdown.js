var ExpressQuiz;
(function (ExpressQuiz) {
    var CountDown = (function () {
        function CountDown(options) {
            this.options = options;
            this._options = options;
            this.updateStatus = options.onUpdateStatus || function () {
            };
            this.counterEnd = options.onCounterEnd || function () {
            };
            this.self = this;
        }
        CountDown.prototype.start = function () {
            clearInterval(this.timer);
            this.timer = 0;
            this.seconds = this._options.seconds;
            this.timer = setInterval(this.decrementCounter, 1000, this);
        };

        CountDown.prototype.stop = function () {
            clearInterval(this.timer);
        };

        CountDown.prototype.getRemainingTime = function () {
            return this._options.seconds - this.seconds;
        };

        CountDown.prototype.decrementCounter = function (self) {
            self.updateStatus(self.seconds);
            if (self.seconds === 0) {
                self.counterEnd();
                self.stop();
            }
            self.seconds--;
        };
        return CountDown;
    })();
    ExpressQuiz.CountDown = CountDown;
})(ExpressQuiz || (ExpressQuiz = {}));
//# sourceMappingURL=countdown.js.map
