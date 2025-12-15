/**
 * Service to fetch books from backend API
 * Handles pagination, sorting, and filtering
 */

export class BooksService {
  constructor() {
    this.baseUrl = '/api/v1/books';
  }

  /**
   * Fetch books with pagination, sorting, and filtering
   * @param {Object} params - Query parameters
   * @param {number} params.page - Page number (1-indexed)
   * @param {number} params.pageSize - Items per page
   * @param {string} params.sortBy - Sort field (price, title, createdAt, etc.)
   * @param {string} params.sortOrder - 'asc' or 'desc'
   * @param {Object} params.filters - Filter object with keys like brand, availability, supplier, minPrice, maxPrice
   * @returns {Promise<Object>} - { items: [], totalCount, currentPage, totalPages }
   */
  async getBooks(params = {}) {
    const {
      page = 1,
      pageSize = 9,
      sortBy = 'title',
      sortOrder = 'asc',
      filters = {}
    } = params;

    // Build query string
    const queryParams = new URLSearchParams();
    queryParams.append('page', page);
    queryParams.append('pageSize', pageSize);
    queryParams.append('sortBy', sortBy);
    queryParams.append('sortOrder', sortOrder);

    // Add filter parameters
    Object.entries(filters).forEach(([key, value]) => {
      if (value !== null && value !== undefined && value !== '') {
        queryParams.append(key, value);
      }
    });

    const url = `${this.baseUrl}?${queryParams.toString()}`;

    try {
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          ...this.getAuthHeaders()
        }
      });

      if (!response.ok) {
        throw new Error(`API error: ${response.status}`);
      }

      return await response.json();
    } catch (error) {
      console.error('Failed to fetch books:', error);
      throw error;
    }
  }

    /**
   * Fetch single book by ID
   * @param {string|number} id
   * @returns {Promise<Object>}
   */
    async getBookById(id) {
        const url = `${this.baseUrl}/${id}`;


        try {
            const response = await fetch(url, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    ...this.getAuthHeaders()
                }
            });

            if (!response.ok) {
                throw new Error(`API error: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('Failed to fetch book:', error);
            throw error;
        }
    }


  /**
   * Get authorization headers if user is logged in
   */
  getAuthHeaders() {
    const token = localStorage.getItem('token');
    if (token) {
      return {
        'Authorization': `Bearer ${token}`
      };
    }
    return {};
  }
}

export default new BooksService();
