﻿using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class BrandManager : IBrandService
    {
        IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        private IResult CheckIfBrandExists(Brand brand)
        {
            if (_brandDal.Get(b => b.Name == brand.Name) != null)
            {
                return new ErrorResult(Messages.BrandAlreadyExists);
            }
            return new SuccessResult();
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IBrandService.Get")]
        [ValidationAspect(typeof(BrandValidator))]
        public IResult Add(Brand brand)
        {
            var businessResult = BusinessRules.Run(CheckIfBrandExists(brand));
            if (businessResult != null)
            {
                return businessResult;
            }
            return new SuccessResult(Messages.BrandAdded);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IBrandService.Get")]
        public IResult Delete(Brand brand)
        {
            _brandDal.Delete(brand);
            return new SuccessResult(Messages.BrandDeleted);
        }

        [CacheAspect(10)]
        public IDataResult<Brand> Get(int brandId)
        {
            return new SuccessDataResult<Brand>(_brandDal.Get(b => b.Id == brandId));
        }

        [CacheAspect(10)]
        public IDataResult<List<Brand>> GetAll()
        {
            return new SuccessDataResult<List<Brand>>(_brandDal.GetAll());
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IBrandService.Get")]
        [ValidationAspect(typeof(BrandValidator))]
        public IResult Update(Brand brand)
        {
            _brandDal.Update(brand);
            return new SuccessResult(Messages.BrandUpdated);
        }
    }
}
