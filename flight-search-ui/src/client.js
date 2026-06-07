class FlightSearchClient {
    /**
     * @param {string} baseUrl - The base URL of the API (e.g., 'https://api.example.com')
     */
    constructor(baseUrl = '') {
      this.baseUrl = baseUrl.replace(/\/$/, ''); // Remove trailing slash if present
    }
  
    /**
     * Helper method to handle HTTP requests and responses
     */
    async _request(endpoint, options = {}) {
      const url = `${this.baseUrl}${endpoint}`;
      
      const headers = {
        'Content-Type': 'application/json',
        ...options.headers,
      };
  
      const config = {
        ...options,
        headers,
      };
  
      try {
        const response = await fetch(url, config);
        
        if (!response.ok) {
          throw new Error(`API Error: ${response.status} ${response.statusText}`);
        }
  
        // If response has content, parse it as JSON
        const contentType = response.headers.get('content-type');
        if (contentType && contentType.includes('application/json')) {
          return await response.json();
        }
        
        return null; 
      } catch (error) {
        console.error(`Failed to fetch from ${endpoint}:`, error);
        throw error;
      }
    }
  
    /**
     * GET /api/Flight/origins
     * Fetches available flight origins.
     */
    async getOrigins() {
      return this._request('/api/Flight/origins', {
        method: 'GET',
      });
    }
  
    /**
     * POST /api/Flight/destinations
     * Fetches destinations based on the provided origin.
     * * @param {Object} data
     * @param {string|null} data.origin
     */
    async getDestinations(data) {
      return this._request('/api/Flight/destinations', {
        method: 'POST',
        body: JSON.stringify({
          origin: data?.origin ?? null
        }),
      });
    }
  
    /**
     * POST /api/Flight/search
     * Searches for flights based on criteria.
     * * @param {Object} data
     * @param {string|null} data.origin
     * @param {string|null} data.destination
     * @param {string|null} data.departureDate - Format: ISO date-time string
     * @param {string|null} data.tripType
     */
    async searchFlights(data) {
      return this._request('/api/Flight/search', {
        method: 'POST',
        body: JSON.stringify({
          origin: data?.origin ?? null,
          destination: data?.destination ?? null,
          departureDate: data?.departureDate ?? null,
          tripType: data?.tripType ?? null,
        }),
      });
    }
  }

  export default FlightSearchClient;