$(document).ready(function () {
    var egg = new Egg("c,o,d,e,r,a,n,t,i,n,e", function () {
        var rythm = new Rythm();
        rythm.setMusic('res/music.mp3');
        rythm.start();
        setTimeout(function () { rythm.stop(false); }, 10000);
    }).listen();

    $(window).scroll(function () {
        RecalculateConfirmPercentage();
    });

    $(window).resize(function () {
        RecalculateConfirmPercentage();
    });

    function RecalculateConfirmPercentage() {

        var scrolToTop = $(window).scrollTop() + $(window).height();
        var anchorHeightFromTop = $('#confirm-scroll-anchor').offset().top;

        var confirmMessageElem = $('#confirm-message');
        if (confirmMessageElem.is(":visible")) {
            anchorHeightFromTop -= confirmMessageElem.outerHeight();
        }

        if (scrolToTop > anchorHeightFromTop) {
            scrolToTop = anchorHeightFromTop;
        }

        var percentage = Math.ceil(scrolToTop / anchorHeightFromTop * 100);
        if (percentage > 100) {
            percentage = 100;
        }

        $('#confirm-progress-bar').width(percentage + '%');

        if (percentage === 100) {
            confirmMessageElem.show();
        } else {
            confirmMessageElem.hide();
        }
    }
});