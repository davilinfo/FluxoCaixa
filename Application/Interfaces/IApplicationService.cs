namespace Application.Interfaces
{
  public interface IApplicationService<Model> where Model : class
  {    
    public Task<Model> GetById(Guid id);

    public IEnumerable<Model> GetByEmail(string email);

    public Task<Model> Add(Model model);    

    public Task<Model> Update(Model model);    
  }
}
