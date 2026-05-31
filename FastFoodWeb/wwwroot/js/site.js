const nav = document.getElementById('mainNav');
if (nav) {
    window.addEventListener('scroll', () => {
        nav.classList.toggle('scrolled', window.scrollY > 10);
    });
}

const hbg = document.getElementById('hamburger');
const navLinks = document.querySelector('.nav-links');
if (hbg && navLinks) {
    hbg.addEventListener('click', () => {
        hbg.classList.toggle('open');
        navLinks.style.display =
            navLinks.style.display === 'flex' ? 'none' : 'flex';
    });
}

const toast = document.getElementById('toastMsg');
if (toast) {
    setTimeout(() => {
        toast.style.transition = 'opacity .4s';
        toast.style.opacity = '0';
        setTimeout(() => toast.remove(), 400);
    }, 2500);
}
