import accountService from './account-service';

const API_URL = 'http://localhost:7020/api/v1/ratings';

const ratingService = {
  async createRating(ratingData) {
    try {
      const headerData = accountService.getHeaderData();
      const response = await fetch(`${API_URL}/create`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${headerData.accessToken}`
        },
        body: JSON.stringify({
          productId: ratingData.productId,
          userId: ratingData.userId,
          ratingValue: ratingData.ratingValue,
          comment: ratingData.comment
        })
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to create rating');
      }

      return await response.json();
    } catch (error) {
      throw new Error(error.message || 'Failed to create rating');
    }
  },

  async updateRating(ratingData) {
    try {
      const headerData = accountService.getHeaderData();
      const response = await fetch(`${API_URL}/update`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${headerData.accessToken}`
        },
        body: JSON.stringify({
          ratingId: ratingData.ratingId,
          productId: ratingData.productId,
          userId: ratingData.userId,
          ratingValue: ratingData.ratingValue,
          comment: ratingData.comment
        })
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to update rating');
      }

      return await response.json();
    } catch (error) {
      throw new Error(error.message || 'Failed to update rating');
    }
  },

  async deleteRating(ratingId) {
    try {
      const headerData = accountService.getHeaderData();
      const response = await fetch(`${API_URL}/delete?ratingId=${ratingId}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${headerData.accessToken}`
        }
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to delete rating');
      }

      return await response.json();
    } catch (error) {
      throw new Error(error.message || 'Failed to delete rating');
    }
  },

  async getRatingById(ratingId) {
    try {
      const response = await fetch(`${API_URL}/?ratingId=${ratingId}`);

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to get rating');
      }

      return await response.json();
    } catch (error) {
      throw new Error(error.message || 'Failed to get rating');
    }
  }
};

export default ratingService;
