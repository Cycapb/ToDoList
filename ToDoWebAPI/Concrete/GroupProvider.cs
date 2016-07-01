﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoDAL.Abstract;
using ToDoDAL.Model;
using ToDoWebAPI.Abstract;

namespace ToDoWebAPI.Concrete
{
    public class GroupProvider:IEntityValueProvider<Group>
    {
        private readonly IRepository<Group> _repository;

        public GroupProvider(IRepository<Group> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Group>> GetValuesAsync()
        {
            return (await _repository.GetListAsync())
                .Select(x => new Group()
                {
                    GroupId = x.GroupId,
                    Name = x.Name,
                    UserId = x.UserId
                });
        }

        public async Task<Group> GetValueAsync(int id)
        {
            var item = await _repository.GetItemAsync(id);
            if (item == null)
            {
                return null;
            }
            else
            {
                return new Group()
                {
                    GroupId = item.GroupId,
                    Name = item.Name,
                    UserId = item.UserId
                };
            }
        }

        public async Task CreateValueAsync(Group item)
        {
            await _repository.CreateAsync(item);
            await _repository.SaveAsync();
        }

        public async Task UpdateValueAsync(Group item)
        {
            await _repository.UpdateAsync(item);
            await _repository.SaveAsync();
        }

        public async Task UpdateValuesAsync(IEnumerable<Group> items)
        {
            foreach (var group in items)
            {
                await _repository.UpdateAsync(group);
            }
            await _repository.SaveAsync();
        }

        public async Task DeleteValueAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.DeleteAsync(id);
        }
    }
}