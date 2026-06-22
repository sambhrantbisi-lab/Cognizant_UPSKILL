/* Cognizant exercise: JavaScript DOM manipulation, event handling, validation, arrays/objects, closures, storage, fetch, async/await, ES6, jQuery */

const baseEvents = [
  {
    title: 'Community Clean-Up',
    date: 'Saturday, 9:00 AM',
    location: 'Central Park',
    type: 'Volunteering',
    description: 'Join neighbors for a park clean-up, recycling drive, and a small refreshments table after the activity.',
    features: ['Volunteer badges', 'Safety briefing', 'Family-friendly']
  },
  {
    title: 'Food Drive & Donation Hub',
    date: 'Sunday, 11:00 AM',
    location: 'City Hall Courtyard',
    type: 'Social Impact',
    description: 'Collect essential supplies and help with sorting and distribution for local families.',
    features: ['Donation counter', 'Guided support', 'Certificates']
  },
  {
    title: 'Local Talent Fest',
    date: 'Next Friday, 6:30 PM',
    location: 'Riverside Stage',
    type: 'Culture',
    description: 'Music, dance, and spoken-word performances from residents and student groups.',
    features: ['Live stage', 'Food stalls', 'Open seating']
  }
];

const eventCatalog = [...baseEvents];
const eventSelect = document.getElementById('eventType');
const eventsGrid = document.getElementById('eventsGrid');
const form = document.getElementById('registrationForm');
const formMessage = document.getElementById('formMessage');
const charCount = document.getElementById('charCount');
const geoBtn = document.getElementById('geoBtn');
const geoOutput = document.getElementById('geoOutput');
const nearestEvent = document.getElementById('nearestEvent');
const scheduleTable = document.getElementById('scheduleTable');
const refreshEventsBtn = document.getElementById('refreshEventsBtn');
const saveDraftBtn = document.getElementById('saveDraftBtn');
const loadDraftBtn = document.getElementById('loadDraftBtn');
const clearDraftBtn = document.getElementById('clearDraftBtn');
const toastBtn = document.getElementById('toastBtn');

let draftKey = 'community-event-draft';
let registrationKey = 'community-event-registration';
let sessionGreetingKey = 'community-event-session-greeting';

function createBadge(type) {
  const badgeMap = {
    Volunteering: 'success',
    'Social Impact': 'primary',
    Culture: 'warning'
  };

  return badgeMap[type] || 'secondary';
}

function renderEventCards(events) {
  eventsGrid.innerHTML = '';

  events.forEach((eventItem, index) => {
    const col = document.createElement('div');
    col.className = 'col-lg-4 col-md-6';

    col.innerHTML = `
      <article class="card h-100 shadow-sm border-0">
        <div class="card-body d-flex flex-column">
          <div class="d-flex justify-content-between align-items-start gap-2 mb-3">
            <span class="badge text-bg-${createBadge(eventItem.type)}">${eventItem.type}</span>
            <small class="text-muted">#${index + 1}</small>
          </div>
          <h3 class="h4 card-title">${eventItem.title}</h3>
          <p class="text-muted mb-2"><i class="bi bi-clock me-2"></i>${eventItem.date}</p>
          <p class="text-muted mb-3"><i class="bi bi-geo-alt me-2"></i>${eventItem.location}</p>
          <p>${eventItem.description}</p>
          <ul class="event-features small mb-4 ps-3">
            ${eventItem.features.map((feature) => `<li>${feature}</li>`).join('')}
          </ul>
          <div class="mt-auto d-flex gap-2">
            <button class="btn btn-sm btn-outline-primary flex-grow-1" type="button" data-event="${eventItem.title}">Select</button>
            <button class="btn btn-sm btn-outline-secondary flex-grow-1" type="button" data-detail-index="${index}">Details</button>
          </div>
        </div>
      </article>
    `;

    eventsGrid.appendChild(col);
  });
}

function populateSelect(events) {
  const options = events.map((eventItem) => `<option value="${eventItem.title}">${eventItem.title}</option>`).join('');
  eventSelect.insertAdjacentHTML('beforeend', options);
}

function updateCharacterCount(textarea) {
  charCount.textContent = textarea.value.length;
}

function showMessage(message, type = 'info') {
  formMessage.className = `alert alert-${type} mt-4`;
  formMessage.textContent = message;
  formMessage.classList.remove('d-none');
}

function hideMessage() {
  formMessage.classList.add('d-none');
}

function handleBlur(element) {
  if (element.value.trim()) {
    element.classList.add('is-valid');
    element.classList.remove('is-invalid');
  }
}

function handleInputChange(element) {
  element.classList.remove('is-invalid');
  element.classList.add('is-valid');
}

function handleEventTypeChange(selectElement) {
  if (selectElement.value) {
    nearestEvent.textContent = selectElement.value;
  }
}

function showBannerNote() {
  formMessage.className = 'alert alert-info mt-4';
  formMessage.textContent = 'Welcome note triggered with an inline onclick handler.';
  formMessage.classList.remove('d-none');
}

function clearMessage(textarea) {
  textarea.value = '';
  updateCharacterCount(textarea);
}

function saveFormDraft() {
  const draft = {
    fullName: document.getElementById('fullName').value,
    email: document.getElementById('email').value,
    phone: document.getElementById('phone').value,
    eventType: document.getElementById('eventType').value,
    eventDate: document.getElementById('eventDate').value,
    attendees: document.getElementById('attendees').value,
    message: document.getElementById('message').value
  };

  localStorage.setItem(draftKey, JSON.stringify(draft));
  showMessage('Draft saved in localStorage.', 'success');
}

function loadFormDraft() {
  const draft = localStorage.getItem(draftKey);

  if (!draft) {
    showMessage('No saved draft found.', 'warning');
    return;
  }

  const parsedDraft = JSON.parse(draft);
  document.getElementById('fullName').value = parsedDraft.fullName || '';
  document.getElementById('email').value = parsedDraft.email || '';
  document.getElementById('phone').value = parsedDraft.phone || '';
  document.getElementById('eventType').value = parsedDraft.eventType || '';
  document.getElementById('eventDate').value = parsedDraft.eventDate || '';
  document.getElementById('attendees').value = parsedDraft.attendees || 1;
  document.getElementById('message').value = parsedDraft.message || '';
  updateCharacterCount(document.getElementById('message'));
  showMessage('Draft loaded from localStorage.', 'info');
}

function clearFormDraft() {
  localStorage.removeItem(draftKey);
  showMessage('Saved draft cleared.', 'secondary');
}

function saveRegistration(registrationData) {
  localStorage.setItem(registrationKey, JSON.stringify(registrationData));
}

function getSelectedEventData(eventTitle) {
  return eventCatalog.find((eventItem) => eventItem.title === eventTitle);
}

function renderSelectedEventDetails(eventTitle) {
  const selectedEvent = getSelectedEventData(eventTitle) || eventCatalog[0];
  nearestEvent.textContent = selectedEvent.title;
}

function addScheduleRow(registrationData) {
  const row = document.createElement('tr');
  row.innerHTML = `
    <td>${registrationData.eventType}</td>
    <td>${registrationData.eventDate}</td>
    <td>Registered by ${registrationData.fullName}</td>
    <td><span class="badge text-bg-success">Confirmed</span></td>
  `;
  scheduleTable.prepend(row);
}

function validateRegistrationData() {
  const fullName = document.getElementById('fullName');
  const email = document.getElementById('email');
  const phone = document.getElementById('phone');
  const eventType = document.getElementById('eventType');
  const eventDate = document.getElementById('eventDate');
  const attendees = document.getElementById('attendees');

  const fields = [fullName, email, phone, eventType, eventDate, attendees];
  let isValid = true;

  fields.forEach((field) => {
    if (!field.checkValidity()) {
      field.classList.add('is-invalid');
      field.classList.remove('is-valid');
      isValid = false;
    } else {
      field.classList.add('is-valid');
      field.classList.remove('is-invalid');
    }
  });

  return isValid;
}

function collectRegistrationData() {
  const { value: fullName } = document.getElementById('fullName');
  const { value: email } = document.getElementById('email');
  const { value: phone } = document.getElementById('phone');
  const { value: eventType } = document.getElementById('eventType');
  const { value: eventDate } = document.getElementById('eventDate');
  const { value: attendees } = document.getElementById('attendees');
  const { value: message } = document.getElementById('message');

  return {
    fullName: fullName.trim(),
    email: email.trim(),
    phone: phone.trim(),
    eventType,
    eventDate,
    attendees: Number(attendees),
    message: message.trim()
  };
}

function initGeolocation() {
  geoBtn.addEventListener('click', () => {
    if (!navigator.geolocation) {
      geoOutput.textContent = 'Geolocation not supported in this browser.';
      return;
    }

    navigator.geolocation.getCurrentPosition(
      (position) => {
        const latitude = position.coords.latitude.toFixed(4);
        const longitude = position.coords.longitude.toFixed(4);
        geoOutput.textContent = `${latitude}, ${longitude}`;
        nearestEvent.textContent = 'Community Meetup Near You';
      },
      (error) => {
        geoOutput.textContent = `Unable to fetch location: ${error.message}`;
      },
      { enableHighAccuracy: true, timeout: 8000 }
    );
  });
}

async function fetchCommunityTips() {
  try {
    const tipsDataUrl = 'data:application/json,{"tips":["Arrive 15 minutes early.","Carry a water bottle.","Keep your registration email handy."]}';
    const response = await fetch(tipsDataUrl);

    if (!response.ok) {
      throw new Error('Failed to load community tips.');
    }

    const data = await response.json();
    const tipList = document.createElement('ul');
    tipList.className = 'mt-3 small text-muted';
    data.tips.forEach((tip) => {
      const item = document.createElement('li');
      item.textContent = tip;
      tipList.appendChild(item);
    });
    document.querySelector('.site-footer .container').appendChild(tipList);
  } catch (error) {
    console.error(error);
  }
}

async function initAsyncDemo() {
  try {
    const currentYear = await Promise.resolve(new Date().getFullYear());
    document.querySelector('.site-footer p').textContent = `Local Community Event Portal © ${currentYear}`;
  } catch (error) {
    console.error('Async init failed', error);
  }
}

function setupEventDelegation() {
  eventsGrid.addEventListener('click', (event) => {
    const selectButton = event.target.closest('[data-event]');
    const detailButton = event.target.closest('[data-detail-index]');

    if (selectButton) {
      const selectedTitle = selectButton.dataset.event;
      eventSelect.value = selectedTitle;
      renderSelectedEventDetails(selectedTitle);
      showMessage(`Selected ${selectedTitle} for registration.`, 'info');
    }

    if (detailButton) {
      const selectedEvent = eventCatalog[Number(detailButton.dataset.detailIndex)];
      alert(`${selectedEvent.title}\n${selectedEvent.description}`);
    }
  });
}

function registerFormEvents() {
  form.addEventListener('submit', (event) => {
    event.preventDefault();

    if (!validateRegistrationData()) {
      showMessage('Please correct the highlighted fields.', 'danger');
      return;
    }

    const registrationData = collectRegistrationData();
    saveRegistration(registrationData);
    addScheduleRow(registrationData);
    showMessage(`Registration complete for ${registrationData.fullName}.`, 'success');
    form.reset();
    charCount.textContent = '0';
  });

  saveDraftBtn.addEventListener('click', saveFormDraft);
  loadDraftBtn.addEventListener('click', loadFormDraft);
  clearDraftBtn.addEventListener('click', clearFormDraft);

  document.getElementById('message').addEventListener('input', (event) => {
    updateCharacterCount(event.target);
  });

  document.getElementById('fullName').addEventListener('dblclick', () => {
    document.getElementById('fullName').value = '';
  });

  document.getElementById('email').addEventListener('keypress', (event) => {
    if (event.key === 'Enter') {
      event.preventDefault();
      saveFormDraft();
    }
  });
}

function renderInitialData() {
  renderEventCards(eventCatalog);
  populateSelect(eventCatalog);
  renderSelectedEventDetails(eventCatalog[0].title);
}

function applySessionGreeting() {
  if (!sessionStorage.getItem(sessionGreetingKey)) {
    sessionStorage.setItem(sessionGreetingKey, 'shown');
    showMessage('Welcome to the Local Community Event Portal. Your session is active.', 'info');
  }
}

function wireBootstrapVisibilityDemo() {
  const footerNote = document.createElement('p');
  footerNote.className = 'mt-3 d-none d-sm-block small text-muted';
  footerNote.textContent = 'This note uses a Bootstrap responsive visibility class.';
  document.querySelector('.site-footer .container').appendChild(footerNote);
}

function initJQueryInteractions() {
  $(document).ready(() => {
    $('#toastBtn').on('click', () => {
      $('#formMessage').stop(true, true).hide().removeClass('alert-danger alert-warning alert-success').addClass('alert-info').text('Welcome! Use the sections above to explore every exercise.').fadeIn(200).delay(1200).fadeOut(200);
    });

    $('#saveDraftBtn').on('mouseenter', () => $('#saveDraftBtn').toggleClass('shadow'));
    $('#saveDraftBtn').on('mouseleave', () => $('#saveDraftBtn').toggleClass('shadow'));

    $('#clearDraftBtn').on('dblclick', () => {
      $('#message').val('');
      $('#charCount').text('0');
      $('#formMessage').hide().text('Textarea cleared by double click.').fadeIn(150);
    });

    $('#fullName').on('keypress', (event) => {
      if (event.key === 'Enter') {
        event.preventDefault();
        $('#email').focus();
      }
    });

    $('#refreshEventsBtn').on('click', () => {
      $('#eventsGrid').hide().fadeIn(250);
      renderEventCards(eventCatalog);
    });

    $('#geoBtn').on('click', () => {
      $('#geoOutput').fadeOut(50).fadeIn(250);
    });

    $('#registrationForm .btn-primary').on('click', () => {
      $('#registrationForm').toggleClass('was-validated');
    });
  });
}

function demoClosures() {
  function createSubmissionCounter() {
    let count = 0;

    return function incrementCounter() {
      count += 1;
      return count;
    };
  }

  const nextCount = createSubmissionCounter();
  console.debug('Submission counter initialized at', nextCount());
}

function bootstrapUtilitiesDemo() {
  const utilitySnippet = document.createElement('div');
  utilitySnippet.className = 'd-flex justify-content-between align-items-center mt-4 p-3 border rounded bg-light';
  utilitySnippet.innerHTML = `
    <span class="fw-semibold">Bootstrap flexbox utilities in use</span>
    <span class="badge text-bg-success">Responsive</span>
  `;
  document.querySelector('#contact .container').appendChild(utilitySnippet);
}

function main() {
  try {
    renderInitialData();
    setupEventDelegation();
    registerFormEvents();
    initGeolocation();
    applySessionGreeting();
    wireBootstrapVisibilityDemo();
    initJQueryInteractions();
    bootstrapUtilitiesDemo();
    demoClosures();
    fetchCommunityTips();
    initAsyncDemo();
  } catch (error) {
    console.error('Initialization error:', error);
  }
}

main();

// Cognizant exercise: localStorage support for loading a saved registration
const savedRegistration = localStorage.getItem(registrationKey);
if (savedRegistration) {
  try {
    const parsed = JSON.parse(savedRegistration);
    document.getElementById('nearestEvent').textContent = parsed.eventType || 'Community Event';
  } catch (error) {
    console.error('Could not parse saved registration', error);
  }
}
