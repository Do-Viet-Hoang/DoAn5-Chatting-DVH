const _PORT = 3000;
const app = require("http").createServer();
const io = require("socket.io").listen(app);
app.listen(_PORT);

const axios = require('axios');

const _urlApi = "http://localhost:55355/api/";

// key send
const _KEY = {
    ON: '1',
    SEND_MESSAGE: '2'
};

var USERS = [];

io.on("connection", (socket) => {
    // on connected
    socket.on(_KEY.ON, (data) => {
        USERS = USERS.filter(user => user.users_id != data.users_id);
        data.socket_id = socket.id;
        USERS.push(data);
        console.log("push: " + data.fullName);
        io.emit(_KEY.ON, USERS);
    });

    socket.on(_KEY.SEND_MESSAGE, (data) => {
        let token = data.from.token;
        // let from_id = data.from.users_id;
        let formData = {
            // fromUserId: from_id,
            toUserId: data.to_id,
            content: data.content,
            MediaFilePath: data.media,
        };
        axios.post(_urlApi + "Meessages/send-message", formData, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
        .then(res => {
            console.log(res.data);
            // send message to socket user
            let user_receive = USERS.filter(user => user.users_id == data.to_id)[0];

            if(user_receive) {
                io.to(user_receive.socket_id).emit(_KEY.SEND_MESSAGE, res.data);
            }
        })
        .catch(err => console.error(err.response.status + ": " + err.response.statusText));
    });
});