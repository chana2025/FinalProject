using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DietTypeService : IService<DietDto>
    {
        private readonly IRepository<DietType> _repository;
        private readonly IMapper _mapper;

        public DietTypeService(IRepository<DietType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public DietDto AddItem(DietDto item)
        {
            string savedImagePath = null;

            if (item.fileImage != null && item.fileImage.Length > 0)
            {
                var fileName = Path.GetFileName(item.fileImage.FileName);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "MyProject", "Images");
                Directory.CreateDirectory(directory); // אם התיקייה לא קיימת, תיווצר
                savedImagePath = Path.Combine(directory, fileName);

                using (var stream = new FileStream(savedImagePath, FileMode.Create))
                {
                    item.fileImage.CopyTo(stream);
                }

                item.ImageUrl = Path.Combine("MyProject", "Images", fileName);

            }

            var dietEntity = _mapper.Map<DietDto, DietType>(item);
            var addedEntity = _repository.AddItem(dietEntity);
            return _mapper.Map<DietType, DietDto>(addedEntity);
        }


        public void DeleteItem(int id)
        {
            _repository.DeleteItem(id);
        }

        public List<DietDto> GetAll()
        {
            return _mapper.Map<List<DietType>, List<DietDto>>(_repository.GetAll());
        }

        public DietDto GetById(int id)
        {
            return _mapper.Map<DietType, DietDto>(_repository.GetById(id));
        }

        public void UpdateItem(int id, DietDto item)
        {
            var existing = _repository.GetById(id);
            if (existing == null) return;

            existing.DietName = item.DietName;
            existing.MealsPerDay = item.MealsPerDay;
            existing.NumCalories = item.NumCalories;
            existing.SpecialComments = item.SpecialComments;
            existing.TimeMealsString = item.TimeMealsString;

            if (item.fileImage != null && item.fileImage.Length > 0)
            {
                var fileName = Path.GetFileName(item.fileImage.FileName);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "MyProject", "Images");
                Directory.CreateDirectory(directory);

                var savedImagePath = Path.Combine(directory, fileName);
                using (var stream = new FileStream(savedImagePath, FileMode.Create))
                {
                    item.fileImage.CopyTo(stream);
                }

                // ⬅️ פה מתעדכן הנתיב החדש
                existing.ImageUrl = Path.Combine("MyProject", "Images", fileName);
            }

            _repository.UpdateItem(id, existing);
        }
    }
    }
