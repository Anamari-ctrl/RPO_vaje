/**
 * Mock Books Service - Returns dummy data for testing
 * Replace with real API calls when backend is ready
 */

const MOCK_BOOKS = [
    { id: 1, title: 'Essential Computer Science', price: 40, stock: 2, image: 'https://via.placeholder.com/240x300?text=Book+1', brand: 'TechPress', supplier: 'Supplier A' },
    { id: 2, title: 'Intro to Computer Science', price: 35, stock: 4, image: 'https://via.placeholder.com/240x300?text=Book+2', brand: 'EdPress', supplier: 'Supplier B' },
    { id: 3, title: 'Algorithms and Data Structures', price: 57, stock: 8, image: 'https://via.placeholder.com/240x300?text=Book+3', brand: 'TechPress', supplier: 'Supplier A' },
    { id: 4, title: 'Programming Essentials', price: 22, stock: 0, image: 'https://via.placeholder.com/240x300?text=Book+4', brand: 'CodePress', supplier: 'Supplier C' },
    { id: 5, title: 'Python Mastery', price: 29, stock: 1, image: 'https://via.placeholder.com/240x300?text=Book+5', brand: 'DevPress', supplier: 'Supplier B' },
    { id: 6, title: 'Foundations of CS', price: 48, stock: 5, image: 'https://via.placeholder.com/240x300?text=Book+6', brand: 'AcadPress', supplier: 'Supplier A' },
    { id: 7, title: 'Web Development Guide', price: 38, stock: 2, image: 'https://via.placeholder.com/240x300?text=Book+7', brand: 'WebPress', supplier: 'Supplier D' },
    { id: 8, title: 'JavaScript: The Good Parts', price: 32, stock: 3, image: 'https://via.placeholder.com/240x300?text=Book+8', brand: 'CodePress', supplier: 'Supplier C' },
    { id: 9, title: 'React for Beginners', price: 45, stock: 6, image: 'https://via.placeholder.com/240x300?text=Book+9', brand: 'DevPress', supplier: 'Supplier B' },
    { id: 10, title: 'Vue.js Complete Guide', price: 52, stock: 4, image: 'https://via.placeholder.com/240x300?text=Book+10', brand: 'WebPress', supplier: 'Supplier D' },
    { id: 11, title: 'Node.js in Action', price: 41, stock: 7, image: 'https://via.placeholder.com/240x300?text=Book+11', brand: 'BackendPress', supplier: 'Supplier E' },
    { id: 12, title: 'Database Design', price: 55, stock: 3, image: 'https://via.placeholder.com/240x300?text=Book+12', brand: 'TechPress', supplier: 'Supplier A' },
    { id: 13, title: 'Cloud Computing Basics', price: 39, stock: 5, image: 'https://via.placeholder.com/240x300?text=Book+13', brand: 'CloudPress', supplier: 'Supplier F' },
    { id: 14, title: 'Machine Learning 101', price: 60, stock: 2, image: 'https://via.placeholder.com/240x300?text=Book+14', brand: 'AIPress', supplier: 'Supplier G' },
    { id: 15, title: 'TypeScript Handbook', price: 36, stock: 8, image: 'https://via.placeholder.com/240x300?text=Book+15', brand: 'TypePress', supplier: 'Supplier C' },
    { id: 16, title: 'Docker for Developers', price: 44, stock: 4, image: 'https://via.placeholder.com/240x300?text=Book+16', brand: 'DevOpsPress', supplier: 'Supplier H' },
    { id: 17, title: 'Git and GitHub Guide', price: 28, stock: 9, image: 'https://via.placeholder.com/240x300?text=Book+17', brand: 'CodePress', supplier: 'Supplier C' },
    { id: 18, title: 'REST API Design', price: 42, stock: 5, image: 'https://via.placeholder.com/240x300?text=Book+18', brand: 'APIPress', supplier: 'Supplier I' },
    { id: 19, title: 'GraphQL Essentials', price: 38, stock: 1, image: 'https://via.placeholder.com/240x300?text=Book+19', brand: 'APIPress', supplier: 'Supplier I' },
    { id: 20, title: 'Testing Best Practices', price: 34, stock: 6, image: 'https://via.placeholder.com/240x300?text=Book+20', brand: 'QAPress', supplier: 'Supplier J' },
    { id: 21, title: 'Clean Code', price: 50, stock: 10, image: 'https://via.placeholder.com/240x300?text=Book+21', brand: 'CodePress', supplier: 'Supplier C' },
    { id: 22, title: 'Design Patterns', price: 47, stock: 3, image: 'https://via.placeholder.com/240x300?text=Book+22', brand: 'ArchPress', supplier: 'Supplier K' },
    { id: 23, title: 'Security Fundamentals', price: 51, stock: 2, image: 'https://via.placeholder.com/240x300?text=Book+23', brand: 'SecPress', supplier: 'Supplier L' },
    { id: 24, title: 'Microservices Architecture', price: 58, stock: 4, image: 'https://via.placeholder.com/240x300?text=Book+24', brand: 'ArchPress', supplier: 'Supplier K' },
];

export class MockBooksService {
  /**
   * Mock fetch books with pagination, sorting, and filtering
   */
  async getBooks(params = {}) {
    const {
      page = 1,
      pageSize = 9,
      sortBy = 'title',
      sortOrder = 'asc',
      filters = {}
    } = params;

    // Simulate network delay
    await new Promise(resolve => setTimeout(resolve, 300));

    let results = [...MOCK_BOOKS];

    // Apply search filter
    if (filters.search) {
      const search = filters.search.toLowerCase();
      results = results.filter(book =>
        book.title.toLowerCase().includes(search) ||
        book.brand.toLowerCase().includes(search)
      );
    }

    // Apply price filter
    if (filters.minPrice !== null && filters.minPrice !== undefined) {
      results = results.filter(book => book.price >= filters.minPrice);
    }
    if (filters.maxPrice !== null && filters.maxPrice !== undefined) {
      results = results.filter(book => book.price <= filters.maxPrice);
    }

    // Apply availability filter
    if (filters.inStock === 'true') {
      results = results.filter(book => book.stock > 0);
    }

    // Apply brand filter
    if (filters.brand) {
      results = results.filter(book =>
        book.brand.toLowerCase().includes(filters.brand.toLowerCase())
      );
    }

    // Apply supplier filter
    if (filters.supplier) {
      results = results.filter(book =>
        book.supplier.toLowerCase().includes(filters.supplier.toLowerCase())
      );
    }

    // Apply sorting
    results.sort((a, b) => {
      let aVal = a[sortBy];
      let bVal = b[sortBy];

      if (typeof aVal === 'string') {
        aVal = aVal.toLowerCase();
        bVal = bVal.toLowerCase();
      }

      if (aVal < bVal) return sortOrder === 'asc' ? -1 : 1;
      if (aVal > bVal) return sortOrder === 'asc' ? 1 : -1;
      return 0;
    });

    // Apply pagination
    const totalCount = results.length;
    const totalPages = Math.ceil(totalCount / pageSize);
    const startIndex = (page - 1) * pageSize;
    const items = results.slice(startIndex, startIndex + pageSize);

    return {
      items,
      totalCount,
      currentPage: page,
      totalPages
    };
  }

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

export default new MockBooksService();
