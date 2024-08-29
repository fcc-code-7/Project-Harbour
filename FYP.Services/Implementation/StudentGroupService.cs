using FYP.Db;
using FYP.Entities;
using FYP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services.Implementation
{
    public class StudentGroupService : IStudentGroupService
    {
        private readonly IStudentGroupRepository _repository;

        public StudentGroupService(IStudentGroupRepository repository)
        {
            _repository = repository;
        }

        public async Task<StudentGroup> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<StudentGroup>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(StudentGroup studentGroup)
        {
            await _repository.AddAsync(studentGroup);
        }

        public async Task UpdateAsync(StudentGroup studentGroup)
        {
            await _repository.UpdateAsync(studentGroup);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
