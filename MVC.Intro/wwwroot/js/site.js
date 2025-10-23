// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Paceloader JavaScript
class Paceloader {
    constructor() {
        this.loader = null;
        this.isVisible = false;
        this.init();
    }

    init() {
        // Create loader HTML
        this.createLoader();
        
        // Show loader on page load
        this.show();
        
        // Hide loader when page is fully loaded
        window.addEventListener('load', () => {
            setTimeout(() => this.hide(), 500);
        });

        // Add loading states to forms
        this.addFormLoadingStates();
        
        // Add loading states to links
        this.addLinkLoadingStates();
    }

    createLoader() {
        const loaderHTML = `
            <div id="paceloader" class="paceloader">
                <div class="paceloader-spinner"></div>
                <div class="paceloader-text">Зареждане...</div>
                <div class="paceloader-progress">
                    <div class="paceloader-progress-bar"></div>
                </div>
            </div>
        `;
        
        document.body.insertAdjacentHTML('afterbegin', loaderHTML);
        this.loader = document.getElementById('paceloader');
    }

    show(text = 'Зареждане...') {
        if (this.loader) {
            this.loader.querySelector('.paceloader-text').textContent = text;
            this.loader.classList.remove('hidden');
            this.isVisible = true;
        }
    }

    hide() {
        if (this.loader && this.isVisible) {
            this.loader.classList.add('hidden');
            setTimeout(() => {
                if (this.loader) {
                    this.loader.style.display = 'none';
                }
            }, 500);
            this.isVisible = false;
        }
    }

    addFormLoadingStates() {
        // Add loading state to all forms
        document.addEventListener('submit', (e) => {
            const form = e.target;
            if (form.tagName === 'FORM') {
                const submitBtn = form.querySelector('button[type="submit"], input[type="submit"]');
                if (submitBtn) {
                    submitBtn.classList.add('loading');
                    submitBtn.disabled = true;
                    
                    // Show loader
                    this.show('Запазване...');
                }
            }
        });
    }

    addLinkLoadingStates() {
        // Add loading state to navigation links
        document.addEventListener('click', (e) => {
            const link = e.target.closest('a');
            if (link && link.href && !link.href.startsWith('http') && !link.href.includes('#')) {
                // Only for internal links
                this.show('Навигация...');
            }
        });
    }
}

// Initialize Paceloader when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    window.paceloader = new Paceloader();
});

// Utility functions for manual control
window.showLoader = (text) => {
    if (window.paceloader) {
        window.paceloader.show(text);
    }
};

window.hideLoader = () => {
    if (window.paceloader) {
        window.paceloader.hide();
    }
};

// Add page transition effects
document.addEventListener('DOMContentLoaded', () => {
    // Add fade-in effect to main content
    const mainContent = document.querySelector('main, .container');
    if (mainContent) {
        mainContent.classList.add('page-enter');
        setTimeout(() => {
            mainContent.classList.add('page-enter-active');
        }, 100);
    }

    // Add loading animation to cards
    const cards = document.querySelectorAll('.card');
    cards.forEach((card, index) => {
        card.classList.add('loading');
        setTimeout(() => {
            card.classList.remove('loading');
        }, index * 100 + 200);
    });
});

// Add smooth scrolling
document.addEventListener('DOMContentLoaded', () => {
    const links = document.querySelectorAll('a[href^="#"]');
    links.forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault();
            const targetId = link.getAttribute('href').substring(1);
            const targetElement = document.getElementById(targetId);
            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
});
