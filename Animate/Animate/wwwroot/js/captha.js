var slider;
var bg_slider;
var originX, originY, trail,
    isMouseDown;
var isOk;
var dotnetHelper;

window.capthaFn = {
    getLoginModel: function (localX) {
        return dotnetHelper.invokeMethodAsync('getLoginModel')
            .then(data => {

                data.positionX = localX;
                console.log(data);

                $.ajax({
                    url: "https://localhost:7268/api/Captcha",
                    type: "post",
                    dataType: "json",
                    data: JSON.stringify(data),
                    contentType: 'application/json; charset=utf-8',

                    success: function (res) {
                        console.log(res.mess);

                        dotnetHelper.invokeMethodAsync('ValidateJwt', data)
                            .then(msg => {
                                alert(msg);
                            });                                                   
                    }
                })

            });
    }
};

function initCaptha(dotnetHelper) {
    window.dotnetHelper = dotnetHelper
    slider = getByClassName0("sc_net_slider_icon");

    slider.addEventListener('mousedown', handleDragStart);
    slider.addEventListener('touchstart', handleDragStart);
    document.addEventListener('mousemove', handleDragMove);
    document.addEventListener('touchmove', handleDragMove);
    document.addEventListener('mouseup', handleDragEnd);
    document.addEventListener('touchend', handleDragEnd);
    document.addEventListener('mousedown', function () { return false; });
    document.addEventListener('touchstart', function () { return false; });

    originX, originY, trail = [],
        isMouseDown = false;

    isOk = false;
    loadCaptcha();
}

function loadCaptcha() {
    $.ajax({
        url: "https://localhost:7268/api/Captcha",
        type: "get",
        dataType: "json",
        success: function (json) {
            var bg = createCanvas(280, 155);
            bg.className = 'bg_img';
            bg_slider = createCanvas(62, 155);
            bg_slider.className = 'bg_slider';
            CanvasSetImage(bg, json.background);
            CanvasSetImage(bg_slider, json.slider);
            var doc = document.getElementsByClassName("sc_net_bgimg")[0];
            doc.innerHTML = "";
            doc.appendChild(bg);
            doc.appendChild(bg_slider);

            console.log(json.modelX);
        }
    })
}

function createCanvas(width, height) {
    var canvas = document.createElement('canvas');
    canvas.width = width;
    canvas.height = height;
    return canvas;
};

function CanvasSetImage(_canvas, base64) {

    //获取2d画布对象
    var ctx = _canvas.getContext("2d");
    //创建图片标签
    var _img = document.createElement("img");
    //设置图片地址
    _img.src = base64;

    //ctx.fillRect(0, 0, _canvas.clientWidth, _canvas.clientHeight);
    //ctx.fillStyle = 'rgba(255, 255, 255, 0)';
    _img.onload = function () {
        ctx.drawImage(_img, 0, 0);
    }
    /*           ctx.drawImage(_img, 10, 10);*/
}

function getByClassName0(className) {
    return document.getElementsByClassName(className)[0];
};

function handleDragStart(e) {
    console.log("handleDragStart");
    if (isOk) return;
    originX = e.clientX || e.touches[0].clientX;
    originY = e.clientY || e.touches[0].clientY;
    isMouseDown = true;
};

function handleDragMove(e) {
    if (!isMouseDown) return false;
    var eventX = e.clientX || e.touches[0].clientX;
    var eventY = e.clientY || e.touches[0].clientY;
    var moveX = eventX - originX;
    var moveY = eventY - originY;
    if (moveX >= 0 && moveX <= 243) {
        slider.style.left = moveX + "px";
        bg_slider.style.left = moveX / 243 * 218 + "px";
    }

};

function handleDragEnd(e) {
    if (!isMouseDown)
        return false

    isMouseDown = false
    var eventX = e.clientX || e.changedTouches[0].clientX
    if (eventX == originX)
        return false

    //获取前端的x坐标&#xff1b;
    var a = $(".bg_slider");
    var localX = a[0].offsetLeft;

    capthaFn.getLoginModel(localX);

};