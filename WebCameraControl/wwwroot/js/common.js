function startTimer(displaySpan) {
    //let minutes
    let timer = +displaySpan.getAttribute('data-set-timer-second');

    let interval = setInterval(() => {
        //minutes = +(timer / 60, 10);
        seconds = timer;

        //minutes = minutes < 10 ? '0' + minutes : minutes;
        seconds = seconds < 10 ? '0' + seconds : seconds;

        displaySpan.innerText = 'Time left ' + seconds + 's';

        if (--timer < 0) {
            alert('go back to first page')
            clearInterval(interval);
        }
    }, 1_000);
}