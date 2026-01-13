/**
 * Service to interact with Books API
 * Supports pagination, sorting, and admin CRUD operations
 */
import accountService from './account-service';

const API_BASE_URL = 'http://localhost:7020/api/v1';

export class BooksService {
  /**
   * Fetch books with pagination, sorting, and optional search
   * Maps backend response to UI-friendly shape
   * @param {Object} params
   * @param {number} params.page
   * @param {number} params.pageSize
   * @param {string} params.sortBy - e.g. 'title' or backend column
   * @param {string} params.sortOrder - 'asc' | 'desc'
   * @param {Object} params.filters - currently only `search` is supported by backend
   * @returns {Promise<{items: Array, totalCount: number, currentPage: number, totalPages: number}>}
   */
  async getBooks(params = {}) {
    const {
      page = 1,
      pageSize = 9,
      sortBy = 'title',
      sortOrder = 'asc',
      filters = {}
    } = params;

    const headers = accountService.getHeaderData();

    // Backend expects RequestParameters via POST body
    const sortColumnMap = {
      title: 'ProductName',
      price: 'Price',
      createdAt: 'Created'
    };
    const backendSortColumn = sortColumnMap[sortBy] || sortBy;
    const body = {
      PageNumber: page,
      PageSize: pageSize,
      SearchTerm: filters.search || null,
      CategoryId: filters.categoryId || null,
      GenreId: filters.genreId || null,
      MinPrice: filters.minPrice || null,
      MaxPrice: filters.maxPrice || null,
      InStock: filters.inStock === 'true' || filters.inStock === true ? true : null,
      // Map common UI sort keys to backend columns if needed
      SortColumn: backendSortColumn,
      SortOrder: sortOrder
    };

    const response = await fetch(`${API_BASE_URL}/books`, {
      method: 'POST',
      headers,
      body: JSON.stringify(body)
    });

    if (!response.ok) {
      const error = await response.json().catch(() => ({}));
      throw new Error(error.message || `Failed to fetch books (${response.status})`);
    }

    // Body is a list of ProductResponse items; metadata is in X-Pagination header
    const metaRaw = response.headers.get('X-Pagination');
    let meta = { TotalCount: 0, CurrentPage: page, TotalPages: 1 };
    try { if (metaRaw) meta = JSON.parse(metaRaw); } catch (e) {
      meta = { TotalCount: meta.TotalCount ?? 0, CurrentPage: page, TotalPages: 1 };
    }

      const data = await response.json();
      console.log(data); // preveri, kaj dejansko pride

      const itemsArray = Array.isArray(data.items) ? data.items : data;

      const items = itemsArray.map(p => ({
          id: p.productId,
          title: p.productName,
          price: p.price,
          image: p.imageUrl,
          shortDescription: p.shortDescription,
          stock: p.stock || (p.isAvailable ? 1 : 0)
      }));


    return {
      items,
      totalCount: meta.TotalCount ?? 0,
      currentPage: meta.CurrentPage ?? page,
      totalPages: meta.TotalPages ?? 1
    };
  }

  /**
   * Fetch single book by id (GUID)
   * Backend expects query param `productId`
   * @param {string} id
   * @returns {Promise<Object>} UI-shaped book object
   */
  async getBookById(id) {
    const headers = accountService.getHeaderData();

    const response = await fetch(`${API_BASE_URL}/books?productId=${encodeURIComponent(id)}`, {
      method: 'GET',
      headers
    });

    if (!response.ok) {
      const error = await response.json().catch(() => ({}));
      throw new Error(error.message || `Failed to fetch book (${response.status})`);
    }

    const p = await response.json();
      return {
          id: p.productId,
          title: p.productName,
          price: p.price,
          imageUrl: p.imageUrl,
          isAvailable: p.isAvailable,
          stock: p.stock || (p.isAvailable ? 1 : 0),
          warranty: p.warranty, // <-- DODAJ
          officialUrl: p.manufacturerPageUrl, // <-- DODAJ (backend polje)
          longDescription: p.longDescription,
          technicalDetails: p.technicalDetails
      };


  }

  /**
   * Fetch all genres
   * @returns {Promise<Array>}
   */
  async getGenres() {
    const headers = accountService.getHeaderData();
    const response = await fetch(`${API_BASE_URL}/genres`, {
      method: 'GET',
      headers
    });
    if (!response.ok) throw new Error('Failed to fetch genres');
    return await response.json();
  }

  /**
   * Fetch all categories
   * @returns {Promise<Array>}
   */
  async getCategories() {
    const headers = accountService.getHeaderData();
    const response = await fetch(`${API_BASE_URL}/categories/`, {
      method: 'GET',
      headers
    });
    if (!response.ok) throw new Error('Failed to fetch categories');
    return await response.json();
  }

  /**
   * Create a new book (admin)
   * NOTE: Backend must expose POST /api/v1/books for creation.
   * @param {Object} book - payload compatible with backend ProductAddRequest
   */
  async createBook(book) {
    const headers = accountService.getHeaderData();

    const response = await fetch(`${API_BASE_URL}/books`, {
      method: 'POST',
      headers,
      body: JSON.stringify(book)
    });

    if (!response.ok) {
      const error = await response.json().catch(() => ({}));
      throw new Error(error.message || `Failed to create book (${response.status})`);
    }

    return await response.json();
  }

  /**
   * Update a book (admin)
   * @param {string} id
   * @param {Object} book - payload compatible with backend ProductUpdateRequest
   */
  async updateBook(id, book) {
    const headers = accountService.getHeaderData();

    const response = await fetch(`${API_BASE_URL}/books/${encodeURIComponent(id)}`, {
      method: 'PUT',
      headers,
      body: JSON.stringify(book)
    });

    if (!response.ok) {
      const error = await response.json().catch(() => ({}));
      throw new Error(error.message || `Failed to update book (${response.status})`);
    }

    return await response.json();
  }

  /**
   * Delete a book (admin)
   * @param {string} id
   */
  async deleteBook(id) {
    const headers = accountService.getHeaderData();

    const response = await fetch(`${API_BASE_URL}/books/${encodeURIComponent(id)}`, {
      method: 'DELETE',
      headers
    });

    if (!response.ok) {
      const error = await response.json().catch(() => ({}));
      throw new Error(error.message || `Failed to delete book (${response.status})`);
    }

    return true;
  }

  /**
   * Decrease stock for a product
   * @param {string} productId - The product ID
   * @param {number} quantity - The quantity to decrease
   */
  async decreaseStock(productId, quantity) {
    const headers = accountService.getHeaderData();

    const response = await fetch(`${API_BASE_URL}/books/${encodeURIComponent(productId)}/decrease-stock`, {
      method: 'POST',
      headers,
      body: JSON.stringify({ quantity })
    });

    if (!response.ok) {
      const error = await response.json().catch(() => ({}));
      throw new Error(error.message || `Failed to decrease stock (${response.status})`);
    }

    return await response.json();
  }
}

export default new BooksService();
