function startTimer(displaySpan, callBack) {
    let seconds;
    let timer = +displaySpan.getAttribute('data-set-timer-second');
    let interval = setInterval(() => {
        seconds = timer;

        //minutes = minutes < 10 ? '0' + minutes : minutes;
        seconds = seconds < 10 ? '0' + seconds : seconds;

        displaySpan.innerText = 'Time left ' + seconds + 's';

        if (--timer < 0) {
            callBack();
            clearInterval(interval);
        }
    }, 1_000);
}