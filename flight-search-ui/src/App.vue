<template>
  <div class="container mt-4 mb-5">
    <div class="page-wrapper">
      <div class="search-card">
    <h2 class="mb-4">Flight Search</h2>

    <!-- Trip Type -->
    <div class="mb-4">
      <label class="form-label d-block fw-bold">Trip Type</label>
      <div class="btn-group">
        <button
          class="btn"
          :class="tripType === 'oneway' ? 'btn-primary' : 'btn-outline-primary'"
          @click="tripType = 'oneway'"
        >
          One-way
        </button>

        <button
          class="btn"
          :class="tripType === 'return' ? 'btn-primary' : 'btn-outline-primary'"
          @click="tripType = 'return'"
        >
          Return
        </button>
      </div>
    </div>

    <!-- Origin -->
    <div class="mb-4">
      <label class="form-label d-block fw-bold">Origin Airport</label>
      <select class="form-select" v-model="origin" @change="fetchDestinations">
        <option disabled value="">Select origin</option>
        <option v-for="a in origins" :key="a.code" :value="a.code">
          {{ a.name }} ({{ a.code }})
        </option>
      </select>
    </div>

    <!-- Destination -->
    <div class="mb-4">
      <label class="form-label d-block fw-bold">Destination Airport</label>

      <div v-if="loading" class="alert alert-secondary py-2 small">
        Loading destinations…
      </div>

      <div v-if="error" class="alert alert-danger py-2 small">
        {{ error }}
      </div>

      <select
        class="form-select"
        v-model="destination"
        :disabled="loading || !destinations.length"
      >
        <option disabled value="">Select destination</option>
        <option v-for="d in destinations" :key="d.code" :value="d.code">
          {{ d.name }} ({{ d.code }})
        </option>
      </select>
    </div>

    <!-- Dates -->
    <div class="mb-4 d-flex flex-column align-items-center">
      <div v-if="tripType === 'oneway'" class="w-100">
        <label class="form-label d-block fw-bold">Departure Date</label>
        <input type="date" class="form-control mx-auto" v-model="departureDate" />
    </div>
  
  <!-- If return is selected, stack them neatly -->
  <div v-else class="w-100 d-flex flex-column gap-3">
    <div>
      <label class="form-label d-block fw-bold">Departure Date</label>
      <input type="date" class="form-control mx-auto" v-model="departureDate" />
    </div>
    <div>
      <label class="form-label d-block fw-bold">Return Date</label>
      <input type="date" class="form-control mx-auto" v-model="returnDate" />
    </div>
  </div>
</div>

    <!-- Passengers -->
<div class="mb-4">
  <label class="form-label d-block fw-bold">Passengers</label>
  <input type="number" min="1" class="form-control" v-model="passengers" />
</div>

    <!-- Search -->
<div class="mb-4">
  <button class="btn btn-success" @click="search">
    Search Flights
  </button>
</div>


<!-- Results -->
<div v-if="results && results.length > 0" class="mt-4">
  <h5 class="fw-bold mb-3">Available Flights ({{ results.length }})</h5>
  
  <!-- Loop through each flight in the results array -->
  <div v-for="flight in results" :key="flight.id" class="alert alert-info mb-3">
    <ul class="mb-0">
      <li><strong>Airline:</strong> {{ flight.airline }} ({{ flight.flightNumber }})</li>
      <li><strong>Trip:</strong> {{ flight.tripType }}</li>
      <li><strong>Route:</strong> {{ flight.origin }} ({{ flight.originFullName }}) → {{ flight.destination }}</li>
      <li><strong>Departure:</strong> {{ flight.departureDate }} at {{ flight.departureTime }}</li>
      <li><strong>Price:</strong> {{ flight.price }} {{ flight.currency }}</li>
      <li><strong>Seats Left:</strong> {{ flight.availableSeats }}</li>
    </ul>
  </div>
</div>

<!-- Optional: Handle empty state -->
<div v-else-if="results && results.length === 0" class="alert alert-warning mt-4">
  No flights found matching your criteria.
</div>
  </div>
</div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import FlightSearchClient from './client'

const tripType = ref('oneway')
const origin = ref('')
const destination = ref('')
const departureDate = ref('')
const returnDate = ref('')
const passengers = ref(1)

const client = new FlightSearchClient('https://localhost:44381')

const origins = ref([])
const destinations = ref([])
const loading = ref(false)
const error = ref('')
const results = ref(null) 


onMounted(async () => {
  try {
    loading.value = true
    origins.value = await client.getOrigins()
    console.log('Origins:', origins.value)
  } catch (err) {
    error.value = 'Failed to load origins'
    console.error(err)
  } finally {
    loading.value = false
  }
})

async function fetchDestinations() {
  loading.value = true
  error.value = ''
  destinations.value = []
  destination.value = ''

  try {
    const destinationPayload = { origin: origin.value}
    destinations.value = await client.getDestinations(destinationPayload)
  } catch {
    error.value = 'Failed to load destinations'
  } finally {
    loading.value = false
  }
}

async function search() {
  
  const formattedDate = departureDate.value ? departureDate.value : null
  const searchFlightsPayload = {
    origin: origin.value,
    destination: destination.value,
    departureDate: formattedDate,
    tripType: tripType.value
  }
  const flights = await client.searchFlights(searchFlightsPayload);

  flights.forEach(flight => {
  console.log(`Flight ${flight.flightNumber} from ${flight.origin} to ${flight.destination}`);
  
 
});
   results.value = flights || [];

}
</script>

<style>
.container {
  max-width: 600px;
}

/* Page container */
.container {
  max-width: 650px;
}

/* Section spacing */
h2 {
  font-weight: 600;
  letter-spacing: -0.5px;
}

/* Card-like form sections */
.form-section {
  background: #ffffff;
  border-radius: 10px;
  padding: 20px 24px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.06);
  margin-bottom: 24px;
}

/* Centers the label text precisely above the elements */
.form-label {
  font-weight: 600;
  margin-bottom: 6px;
  color: #0a2a43;
  text-align: center;
}

/* Forces strict widths and centering on ALL interactive inputs */
.form-control,
.form-select,
.btn-group,
.btn-success {
  width: 300px !important;
  max-width: 100%;
  display: flex !important;
  margin-left: auto !important;
  margin-right: auto !important;
}

.form-control:focus,
.form-select:focus {
  border-color: #0d6efd;
  box-shadow: 0 0 0 0.15rem rgba(13,110,253,0.25);
}

/* Buttons */
.btn {
  border-radius: 8px;
  font-weight: 500;
}

.btn-group .btn {
  min-width: 110px;
}

/* Ensure the success button centers text inside itself */
.btn-success {
  justify-content: center;
  align-items: center;
}


/* Alerts */
.alert {
  border-radius: 8px;
}

/* Results panel */
.alert-info ul {
  padding-left: 18px;
  margin-bottom: 0;
}

.alert-info li {
  margin-bottom: 4px;
}

/* Loading + error states */
.alert-secondary,
.alert-danger {
  border-radius: 6px;
  padding: 6px 10px;
  font-size: 0.85rem;
}

/* Responsive tweaks */
@media (max-width: 576px) {
  .btn-group .btn {
    min-width: 100px;
  }
}

/* Full-page centering with gradient background */
.page-wrapper {
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;

  /* Cyan → Ocean Blue gradient */
  background: linear-gradient(
    135deg,
    #00eaff 0%,
    #0099ff 40%,
    #0066cc 100%
  );
}

/* Card container */
.search-card {
  background: rgba(255, 255, 255, 0.92);
  backdrop-filter: blur(6px);
  width: 100%;
  max-width: 650px;
  padding: 32px 36px;
  border-radius: 14px;
  box-shadow: 0 4px 18px rgba(0,0,0,0.12);
}

/* Typography */
h2 {
  font-weight: 600;
  letter-spacing: -0.5px;
  margin-bottom: 24px;
  color: #0a2a43;
}

/* Form elements */
.form-label {
  font-weight: 600;
  margin-bottom: 6px;
  color: #0a2a43;
}

.form-control,
.form-select {
  border-radius: 8px;
  padding: 10px 12px;
  border: 1px solid #d0d5dd;
  transition: border-color 0.2s ease, box-shadow 0.2s ease;
  /* width: 300px; */
}

.form-control:focus,
.form-select:focus {
  border-color: #0d6efd;
  box-shadow: 0 0 0 0.15rem rgba(13,110,253,0.25);
}

.col-form-label {
  padding-top: 0;
  padding-bottom: 0;
  display: flex;
  align-items: center;
}

/* Buttons */
.btn {
  border-radius: 8px;
  font-weight: 500;
  width: 300px;
}


.number {
  width: 280px;
}

/* Alerts */
.alert {
  border-radius: 8px;
}

.form-label {
  font-weight: 600;
  margin-bottom: 6px;
  color: #0a2a43;
  text-align: center; /* Centers the label text above the input */
}

/* Typography */
h2 {
  font-weight: 600;
  letter-spacing: -0.5px;
  margin-bottom: 24px;
  color: #0a2a43;
  text-align: center; /* Add this line to center the heading */
}

/* Mobile adjustments */
@media (max-width: 576px) {
  .search-card {
    padding: 24px;
  }
}
</style>