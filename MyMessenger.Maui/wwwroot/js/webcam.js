let rtcConnection = null;
let myVideoStream = null;
let video = null;
let otherVideo = null;
let context = null;
let streaming = false;

let isConnected = false;
let targetConnectionId = null;

let width = 100;
let height = 0;
const accessToken = localStorage.getItem("accessToken").slice(1, -1);

var mediaConstraints = {
  audio: true,
  video: true,
};

const srConnection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7081/chathub", {
    accessTokenFactory: () => accessToken,
  })
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
}

start();

window.WebCamFunctions = {
  start: (options) => {
    return onStart(options);
  },
  dispose: () => {
    dispose();
  },
  toggleMic: (mute) => {
    if (myVideoStream) {
      myVideoStream
        .getAudioTracks()
        .forEach((track) => (track.enabled = !mute));
    }
  },
  toggleCamera: (off) => {
    if (myVideoStream) {
      myVideoStream.getVideoTracks().forEach((track) => (track.enabled = !off));
    }
  },
};

async function onStart(options) {
  console.log("WebRTC: onStart options received:", options);
  video = document.getElementById(options.videoID);
  width = options.width;
  targetConnectionId = options.connectionId || null;

  if (targetConnectionId) {
    await srConnection.invoke("RegisterSharedId", targetConnectionId);
  }

  createPeerConnection();

  navigator.mediaDevices
    .getUserMedia(mediaConstraints)
    .then(function (stream) {
      video.srcObject = stream;
      var localPlaceholder = document.getElementById("localPlaceholder");
      if (localPlaceholder) localPlaceholder.style.display = "none";
      myVideoStream = stream;

      rtcConnection.addStream(myVideoStream);
    })
    .catch(function (err) {
      console.log("An error occurred: " + err);
    });

  video.addEventListener(
    "canplay",
    function () {
      if (!streaming) {
        height = video.videoHeight / (video.videoWidth / width);

        if (isNaN(height)) {
          height = width / (4 / 3);
        }

        video.setAttribute("width", width);
        video.setAttribute("height", height);
        streaming = true;
      }
    },
    false
  );
  video.muted = true;
}

srConnection.on("Receive", (data) => {
  var message = JSON.parse(data);

  if (message.sdp) {
    if (message.sdp.type == "offer") {
      createPeerConnection();
      rtcConnection
        .setRemoteDescription(new RTCSessionDescription(message.sdp))
        .then(function () {
          return navigator.mediaDevices.getUserMedia(mediaConstraints);
        })
        .then(function (stream) {
          myVideoStream = stream;
          video = document.getElementById("video");
          video.srcObject = stream;

          rtcConnection.addStream(myVideoStream);
          var remotePlaceholder = document.getElementById("remotePlaceholder");
          if (remotePlaceholder) remotePlaceholder.style.display = "none";
          var remotePlaceholder = document.getElementById("remotePlaceholder");
          if (remotePlaceholder) remotePlaceholder.style.display = "none";
        })
        .then(function () {
          return rtcConnection.createAnswer();
        })
        .then(function (answer) {
          return rtcConnection.setLocalDescription(answer);
        })
        .then(function () {
          console.log("WebRTC: Sending SDP to shared ID", targetConnectionId);
          srConnection.invoke(
            "SendToSharedId",
            targetConnectionId,
            JSON.stringify({ sdp: rtcConnection.localDescription })
          );
        });
    } else if (message.sdp.type == "answer") {
      rtcConnection.setRemoteDescription(
        new RTCSessionDescription(message.sdp)
      );
    }
  } else if (message.candidate) {
    rtcConnection.addIceCandidate(new RTCIceCandidate(message.candidate));
  }
});

function createPeerConnection() {
  rtcConnection = new RTCPeerConnection(null);

  rtcConnection.onicecandidate = function (event) {
    if (event.candidate && isConnected) {
      console.log(
        "WebRTC: Sending ICE candidate to shared ID",
        targetConnectionId
      );
      srConnection.invoke(
        "SendToSharedId",
        targetConnectionId,
        JSON.stringify({ candidate: event.candidate })
      );
    }
  };

  rtcConnection.onaddstream = function (event) {
    if (event.stream) {
      otherVideo = document.getElementById("remote");
      if (otherVideo) {
        otherVideo.srcObject = event.stream;
        otherVideo.play();
      } else {
        console.error("otherVideo element not found.");
      }
    } else {
      console.error("No stream available in the event.");
    }
  };

  rtcConnection.onnegotiationneeded = function () {
    if (isConnected) {
      rtcConnection
        .createOffer()
        .then(function (offer) {
          return rtcConnection.setLocalDescription(offer);
        })
        .then(function () {
          console.log("WebRTC: Sending offer to shared ID", targetConnectionId);
          srConnection.invoke(
            "SendToSharedId",
            targetConnectionId,
            JSON.stringify({ sdp: rtcConnection.localDescription })
          );
        });
    }
  };
}
function dispose() {
  rtcConnection.close();
  rtcConnection = null;
  myVideoStream.getTracks().forEach((track) => track.stop());
  myVideoStream = null;
}
