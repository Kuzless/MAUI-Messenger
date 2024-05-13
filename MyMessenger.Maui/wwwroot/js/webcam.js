let rtcConnection = null
let myVideoStream = null
let video = null;
let otherVideo = null
let context = null;
let streaming = false;

let isConnected = false;

let width = 100;
let height = 0;    

var mediaConstraints = {
    audio: true, 
    video: true 
};

const srConnection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7081/chathub", { accessTokenFactory: () => options.AccessToken })
    .configureLogging(signalR.LogLevel.Information)
    .build();

srConnection.onclose(start);

async function start() {
    try {
        await srConnection.start();
        console.log("SignalR Connected.");
        isConnected = true;
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};


window.WebCamFunctions = {
    start: (options) => { onStart(options); }
};

function onStart(options) {
    video = document.getElementById(options.videoID);
    width = options.width;

    createPeerConnection()

    navigator.mediaDevices.getUserMedia(mediaConstraints)
        .then(function (stream) {
            video.srcObject = stream
            myVideoStream = stream

            rtcConnection.addStream(myVideoStream)
        })
        .catch(function (err) {
            console.log("An error occurred: " + err);
        });

    video.addEventListener('canplay', function () {
        if (!streaming) {
            height = video.videoHeight / (video.videoWidth / width);

            if (isNaN(height)) {
                height = width / (4 / 3);
            }

            video.setAttribute('width', width);
            video.setAttribute('height', height);
            streaming = true;
        }
    }, false);
}

srConnection.on("Receive", data => {
    var message = JSON.parse(data)

    if (message.sdp) {
        if (message.sdp.type == 'offer') {
            createPeerConnection()
            rtcConnection.setRemoteDescription(new RTCSessionDescription(message.sdp))
                .then(function () {
                    return navigator.mediaDevices.getUserMedia(mediaConstraints);
                })
                .then(function (stream) {
                    myVideoStream = stream
                    video = document.getElementById("video")
                    video.srcObject = stream

                    rtcConnection.addStream(myVideoStream);
                })
                .then(function () {
                    return rtcConnection.createAnswer()
                })
                .then(function (answer) {
                    return rtcConnection.setLocalDescription(answer);
                })
                .then(function () {
                    srConnection.invoke("Send", JSON.stringify({ 'sdp': rtcConnection.localDescription }))
                })
        }
        else if (message.sdp.type == 'answer') {
            rtcConnection.setRemoteDescription(new RTCSessionDescription(message.sdp))
        }
    } else if (message.candidate) {
        rtcConnection.addIceCandidate(new RTCIceCandidate(message.candidate));
    }
});

function createPeerConnection() {
    rtcConnection = new RTCPeerConnection(null)

    rtcConnection.onicecandidate = function (event) {
        if (event.candidate && isConnected) {
            srConnection.invoke("Send", JSON.stringify({ "candidate": event.candidate }));
        }
    }

    rtcConnection.ontrack = function (event) {
        if (event.streams && event.streams[0]) {
            otherVideo.srcObject = event.streams[0];
        } else {
            if (!otherVideo.srcObject) {
                otherVideo.srcObject = new MediaStream();
            }
            otherVideo.srcObject.addTrack(event.track);
        }
    };
    rtcConnection.onnegotiationneeded = function () {
        if (isConnected) {
            rtcConnection.createOffer()
                .then(function (offer) {
                    return rtcConnection.setLocalDescription(offer)
                })
                .then(function () {
                    srConnection.invoke("Send", JSON.stringify({ "sdp": rtcConnection.localDescription }))
                })
        }
    }
}
