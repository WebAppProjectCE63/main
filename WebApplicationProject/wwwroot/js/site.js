document.getElementById('mobileMenuBtn').addEventListener('click', function () {
    var navContent = document.getElementById('navContent');
    if (navContent.classList.contains('show')) {
        navContent.classList.remove('show');
    } else {
        navContent.classList.add('show');
    }
});
