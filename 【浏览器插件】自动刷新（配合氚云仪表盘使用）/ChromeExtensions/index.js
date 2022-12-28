setInterval(() => {
    window.location.reload(true); 
    // 直接使用上面的刷新功能是不能满足当前需求的，原因是因为当每隔十秒钟程序执行的时候 就会将现有的全屏模式推出
}, 10000);