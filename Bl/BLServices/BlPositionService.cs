using Bl.BLApi;
using Bl.BLModels;
using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.BLServices
{
    public class BlPositionService : BlPositionInterface
    {

        DalPositionInterface dal;
        public BlPositionService(DalPositionInterface dal)
        {
            this.dal = dal;
        }
        private BlPositionModel convert(Position p)
        {
            return new BlPositionModel()
            {
                positionCode = p.positionCode,
                positionName = p.positionName
            };
        }
        private Position convert(BlPositionModel p)
        {
            return new Position()
            {
                positionCode = p.positionCode,
                positionName = p.positionName
            };
        }
        private List<BlPositionModel> convert(List<Position> p)
        {
            List<BlPositionModel> list = new List<BlPositionModel>();
            foreach (var item in p)
            {
                list.Add(convert(item));
            }
            return list;
        }

        public void Create(BlPositionModel item)
        {
            dal.Create(convert(item));
        }

        public void Delete(BlPositionModel item)
        {
            dal.Delete(convert(item));
        }
        public async Task<BlPositionModel> Read(int id)
        {
            var data = await dal.Read(id);
            return convert(data);
        }
        public async Task<List<BlPositionModel>> Read(Func<BlPositionModel, bool> func)
        {
            var data = await dal.ReadAll();
            return data
                .Select(convert)
                .Where(func)
                .ToList();
        }
        public async Task<List<BlPositionModel>> ReadAll()
        {
            var data = await dal.ReadAll();
            return data.Select(item => convert(item)).ToList();
        }

        public void Update(BlPositionModel item)
        {
            dal.Update(convert(item));
        }
    }
}
