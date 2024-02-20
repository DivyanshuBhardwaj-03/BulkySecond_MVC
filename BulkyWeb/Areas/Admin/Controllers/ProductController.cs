using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.View_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.product.GetAll().ToList();
           
            return View(objProductList);
        }

        public IActionResult Create()
        {
            // IEnumerable<SelectListItem> CategoryList = _unitOfWork.category.GetAll().Select
            //(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString()
            //});

            // ViewBag.CategoryList = CategoryList;
            // ViewData["CategoryList"]= CategoryList;
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.category.GetAll().Select
           (u => new SelectListItem
           {
               Text = u.Name,
               Value = u.Id.ToString()
           }),
                Product = new Product()

            };
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {

                productVM.CategoryList = _unitOfWork.category.GetAll().Select
       (u => new SelectListItem
       {
           Text = u.Name,
           Value = u.Id.ToString()
       });
                return View(productVM);
            }

            
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product productFromDb = _unitOfWork.product.Get(u => u.Id == id);
            // Product productFromDb = _db.Products.Find(id);
            //Product productFromDb1 = _db.Products.FirstOrDefault(u=>u.Id == id);
            //Product productFromDb2 = _db.Products.Where(u => u.Id == id).FirstOrDefault();

            if (productFromDb == null)
            {
                return View();
            }

            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product productFromDb = _unitOfWork.product.Get(u => u.Id == id);
            // Product productFromDb = _db.Products.Find(id);
            //Product productFromDb1 = _db.Products.FirstOrDefault(u=>u.Id == id);
            //Product productFromDb2 = _db.Products.Where(u => u.Id == id).FirstOrDefault();

            if (productFromDb == null)
            {
                return View();
            }

            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.product.Get(u => u.Id == id);
            //  Product? obj = _db.Products.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");
        }

    }
}
