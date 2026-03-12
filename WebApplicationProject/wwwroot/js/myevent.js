document.addEventListener('DOMContentLoaded', function () {

    const monthSelect = document.getElementById('monthSelect');
    const yearSelect = document.getElementById('yearSelect');
    const calendarGrid = document.getElementById('calendarGrid');
    const searchInput = document.getElementById('searchInput');
    const searchTagInput = document.getElementById('searchTagInput');
    const searchTagBtn = document.getElementById('searchTagBtn');
    const sortSelect = document.getElementById('sortSelect');
    const eventContainer = document.querySelector('.recommend-section');
    const sectionHeader = document.getElementById('sectionHeader');

    let currentSelectedDate = null;

    function renderCalendar() {
        if (!calendarGrid || !monthSelect || !yearSelect) return;

        const year = parseInt(yearSelect.value);
        const month = parseInt(monthSelect.value);
        const firstDay = new Date(year, month, 1).getDay();
        const daysInMonth = new Date(year, month + 1, 0).getDate();

        const today = new Date();
        const todayDateStr = `${today.getFullYear()}-${String(today.getMonth() + 1).padStart(2, '0')}-${String(today.getDate()).padStart(2, '0')}`;

        calendarGrid.innerHTML = '';

        for (let i = 0; i < firstDay; i++) {
            const span = document.createElement('span');
            span.className = 'text-muted';
            calendarGrid.appendChild(span);
        }

        for (let day = 1; day <= daysInMonth; day++) {
            const span = document.createElement('span');
            span.textContent = day;
            span.style.cursor = 'pointer';

            const dateStr = `${year}-${String(month + 1).padStart(2, '0')}-${String(day).padStart(2, '0')}`;

            if (dateStr === todayDateStr) {
                span.classList.add('highlight-day');
                span.title = "Today";
            }
            if (currentSelectedDate === dateStr) {
                span.classList.add('active-day');
            }

            span.addEventListener('click', () => {
                if (currentSelectedDate === dateStr) {
                    currentSelectedDate = null;
                    span.classList.remove('active-day');
                } else {
                    document.querySelectorAll('#calendarGrid span').forEach(s => s.classList.remove('active-day'));
                    span.classList.add('active-day');
                    currentSelectedDate = dateStr;
                }
                applyFilters(); 
            });

            calendarGrid.appendChild(span);
        }
    }

    function applyFilters() {
        const searchText = searchInput ? searchInput.value.toLowerCase().trim() : '';
        const searchTagText = searchTagInput ? searchTagInput.value.toLowerCase().trim() : '';
        const searchTags = searchTagText.split(/[\s,]+/).filter(tag => tag.length > 0);

        let found = 0;
        const currentCards = document.querySelectorAll('.Event');

        currentCards.forEach(card => {
            const dateStr = card.getAttribute('data-date');
            const titleElement = card.querySelector('.event-title');
            const title = titleElement ? titleElement.textContent.toLowerCase() : '';
            const eventTags = Array.from(card.querySelectorAll('.chip')).map(tag => tag.textContent.toLowerCase());

            const matchDate = !currentSelectedDate || dateStr === currentSelectedDate;
            const matchName = !searchText || title.includes(searchText);
            const matchTag = searchTags.length === 0 || searchTags.every(searchTag =>
                eventTags.some(eventTag => eventTag === searchTag) 
            );

            if (matchDate && matchName && matchTag) {
                card.style.display = 'block';
                found++;
            } else {
                card.style.display = 'none';
            }
        });

        if (sectionHeader) {
            sectionHeader.textContent = found > 0 ? "My Events" : "No events found";
        }
    }

    function handleSort() {
        if (!sortSelect || !eventContainer) return;

        const cards = Array.from(document.querySelectorAll('.Event'));
        const sortBy = sortSelect.value;

        cards.sort((a, b) => {
            const dateA = new Date(a.getAttribute('data-date') || 0);
            const dateB = new Date(b.getAttribute('data-date') || 0);

            const memBoxA = a.querySelector('.members');
            const memBoxB = b.querySelector('.members');
            const memA = memBoxA ? parseInt(memBoxA.textContent.split('/')[0]) || 0 : 0;
            const memB = memBoxB ? parseInt(memBoxB.textContent.split('/')[0]) || 0 : 0;

            const titleA = a.querySelector('.event-title') ? a.querySelector('.event-title').textContent.trim() : '';
            const titleB = b.querySelector('.event-title') ? b.querySelector('.event-title').textContent.trim() : '';

            switch (sortBy) {
                case 'date-asc': return dateA - dateB;
                case 'date-desc': return dateB - dateA;
                case 'members-asc': return memA - memB;
                case 'members-desc': return memB - memA;
                case 'name-asc': return titleA.localeCompare(titleB);
                case 'name-desc': return titleB.localeCompare(titleA);
                default: return 0;
            }
        });

        cards.forEach(card => eventContainer.appendChild(card));
    }


    function toggleEventInfo(targetBtn, isOpening) {
        const cardMain = targetBtn.closest('.event-main');
        if (!cardMain) return;

        const extraInfo = cardMain.querySelector('.extra-info');
        const normalActions = cardMain.querySelector('.normal-actions');

        if (extraInfo) extraInfo.style.display = isOpening ? 'block' : 'none';
        if (normalActions) normalActions.style.display = isOpening ? 'none' : 'flex';
    }

    function init() {
        const today = new Date();
        if (monthSelect) monthSelect.value = today.getMonth();
        if (yearSelect) yearSelect.value = today.getFullYear();
        renderCalendar();

        if (monthSelect) monthSelect.addEventListener('change', renderCalendar);
        if (yearSelect) yearSelect.addEventListener('change', renderCalendar);

        if (searchInput) searchInput.addEventListener('input', applyFilters);
        if (searchTagBtn) searchTagBtn.addEventListener('click', applyFilters);
        if (searchTagInput) searchTagInput.addEventListener('input', applyFilters);

        if (sortSelect) {
            sortSelect.addEventListener('change', handleSort);
            handleSort();
        }

        document.addEventListener('click', function (e) {
            if (e.target.classList.contains('view-info-btn')) {
                toggleEventInfo(e.target, true);  
            } else if (e.target.classList.contains('close-info-btn')) {
                toggleEventInfo(e.target, false); 
            }
        });
    }

    init();
});