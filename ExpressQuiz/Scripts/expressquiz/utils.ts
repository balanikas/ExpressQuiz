module ExpressQuiz {
    export class Utils {
      
        public static togglePreventLeavingPage(enable: boolean) {
            if (enable) {
                $(window).on('beforeunload', () =>
                    'All unsaved changes will be lost if you leave or refresh the page.');
            } else {
                $(window).off('beforeunload');
            }
        }
    }
}