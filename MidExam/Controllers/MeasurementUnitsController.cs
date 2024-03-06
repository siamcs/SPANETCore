using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidExam.Data;
using MidExam.Models;
using MidExam.ViewModels;

namespace MidExam.Controllers
{
    public class MeasurementUnitsController : Controller
    {
        private readonly ApplicationDbContext db;

        public MeasurementUnitsController(ApplicationDbContext _db)
        {
         db=_db;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<MeasurementUnitVM> mVMs = db.MeasurementUnits
                .Select(x => new MeasurementUnitVM(x.Id, x.Name));
            return View(await mVMs.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> AddUpdate(MeasurementUnitVM aMeasurementUnitVM, string actionType, string token)
        {

            if (actionType == "add")
            {
                await db.MeasurementUnits.AddAsync(new MeasurementUnit(aMeasurementUnitVM.Name));
                await db.SaveChangesAsync();
            }

            if (actionType == "edit")
            {
                MeasurementUnit measurementUnit = new MeasurementUnit(aMeasurementUnitVM.Id, aMeasurementUnitVM.Name);
                db.MeasurementUnits.Update(measurementUnit);
                await db.SaveChangesAsync();
            }

            string trsWithTds = string.Empty;
            List<MeasurementUnitVM> mVMs = db.MeasurementUnits
                .Select(x => new MeasurementUnitVM(x.Id, x.Name)).ToList();

            if (mVMs.Count > 0)
            {
                foreach (MeasurementUnitVM item in mVMs)
                {
                    trsWithTds += "<tr><td>" + item.Name + "</td><td><button type=\"button\"  data-id=\"" + item.Id + "\" data-name=\"" + item.Name + "\" onclick=\"editMeasurementUnit(this)\">Edit</button><button type=\"button\"  onclick=\"deleteMeasurementUnit('" + token + "'," + item.Id + ")\">Delete</button></td></tr>";
                }
            }

            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> Delete(int id, string token)
        {
            MeasurementUnit measurementUnit = await db.MeasurementUnits.FindAsync(id);

            db.MeasurementUnits.Remove(measurementUnit);
            await db.SaveChangesAsync();

            string trsWithTds = string.Empty;

            List<MeasurementUnitVM> measurementUnitVMs = db.MeasurementUnits
             .Select(x => new MeasurementUnitVM(x.Id, x.Name)).ToList();

            if (measurementUnitVMs.Count > 0)
            {
                foreach (MeasurementUnitVM measurementUnitVM in measurementUnitVMs)
                {
                    trsWithTds += "<tr><td>" + measurementUnitVM.Name + "</td><td><button type=\"button\"  data-id=\"" + measurementUnitVM.Id + "\" data-name=\"" + measurementUnitVM.Name + "\" onclick=\"editMeasurementUnit(this)\">Edit</button><button type=\"button\"  onclick=\"deleteMeasurementUnit('" + token + "'," + measurementUnitVM.Id + ")\">Delete</button></td></tr>";
                }
            }
            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }
    }
}
