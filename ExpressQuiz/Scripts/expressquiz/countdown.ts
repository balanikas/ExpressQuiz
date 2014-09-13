


module ExpressQuiz {
    "use strict";
    export class CountDown
    {
        private timer: any;
        private seconds: number;
        private _options: any;
        private updateStatus: any;
        private counterEnd: any;
        private self: CountDown;
        constructor(private options: any) {
            this._options = options;
            this.updateStatus = options.onUpdateStatus || function() {};
            this.counterEnd = options.onCounterEnd || function () { };
            this.self = this;
        }

        start() : void{
            clearInterval(this.timer);
            this.timer = 0;
            this.seconds = this._options.seconds;
            this.timer = setInterval(this.decrementCounter, 1000, this);
        }

        stop() : void{
            clearInterval(this.timer);
        }

        getRemainingTime() : number{
            return this._options.seconds - this.seconds;
        }

        decrementCounter(self: CountDown) : void{
            self.updateStatus(self.seconds);
            if (self.seconds === 0) {
                self.counterEnd();
                self.stop();
            }
            self.seconds--;
          
        }
    }


}