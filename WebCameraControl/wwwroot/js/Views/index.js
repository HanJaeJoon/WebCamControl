document.addEventListener('DOMContentLoaded', async () => {
    //const stream = await navigator.mediaDevices.getUserMedia({ video: { width: 1280, height: 720 } });
    //const video = document.querySelector('video');

    //video.srcObject = stream;

    navigator.mediaDevices?.enumerateDevices()
        .then(devices => {
            let webcamOption = null;

            for (let i = 0; i < devices.length; i++) {
                const device = devices[i];

                if (device?.kind !== 'videoinput') {
                    continue;
                }

                if (device.label?.toLowerCase()?.includes(variables.CameraManufacturer) || !webcamOption) {
                    webcamOption = {
                        //width: 720,
                        width: 1280,
                        height: 720,
                        image_format: 'jpeg',
                        jpeg_quality: 100,
                        //constraints: {
                        //    width: { exact: 1280 },
                        //    height: { exact: 720 },
                        //},
                        deviceId: { exact: device.deviceId, },
                        flip_horiz: true,

                        // device capture size
                        dest_width: 1280,
                        dest_height: 720,

                        // final cropped size
                        crop_width: 1280,
                        crop_height: 720,
                    };
                }
            };

            Webcam.set(webcamOption);
            Webcam.attach('#camera-preview');
        })
        .catch(err => {
            console.log(err);
        });

    document.querySelector('#btn-take-picture').addEventListener('click', () => {
        Webcam.snap(function (uri) {
            document.querySelector('#picture-list').innerHTML += `<div><img src="${uri}" /></div>`;
        });
    });
});