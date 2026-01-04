namespace WebStore.ServiceContracts
{
    public interface ICommonService<TAdd, TResponse, TUpdate>
    {
        Task<TResponse> CreateItem(TAdd? addRequest);

        Task<List<TResponse>> GetAllItems();

        Task<List<TResponse>> GetActiveItems();

        Task<TResponse?> GetItemById(Guid? itemId);

        Task<TResponse> UpdateItem(TUpdate? updateRequest);

        Task<bool> DeleteItem(Guid? itemId);
    }
}
