namespace WebStore.ServiceContracts
{
    public interface ICommonService<TAdd, TResponse, TUpdate>
    {
        Task<TResponse> CreateItem(TAdd? addRequest, string createdBy);

        Task<List<TResponse>> GetAllItems();

        Task<List<TResponse>> GetActiveItems();

        Task<TResponse?> GetItemById(Guid? itemId);

        Task<TResponse> UpdateItem(TUpdate? updateRequest, string updatedBy);

        Task<bool> DeleteItem(Guid? itemId);

        Task<bool> Deactivate(Guid? itemId, string updatedBy);
    }
}
