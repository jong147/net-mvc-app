using Microsoft.AspNetCore.Mvc;
using MVCApp2.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Azure;
using System.Linq;

namespace MVCApp2.Controllers
{
    [Route("[Controller]")]
    public class PostController : Controller
    {
        [HttpGet]
        [Route("/Post/Index")]
        public async Task<ActionResult> Index(Post? item)
        {
            List<Post> postList = await getPostList();

            if (item != null)
            {
                if (item.id != 0)
                {
                    postList = postList.Where(posts => posts.id == item.id).ToList();
                }
                if (item.title != null && item.title != "")
                {
                    postList = postList.Where(posts => posts.title.Contains(item.title)).ToList();
                }
                if (item.body != null && item.body != "")
                {
                    postList = postList.Where(posts => posts.body.Contains(item.body)).ToList();
                }
                if (item.userId != 0)
                {
                    postList = postList.Where(posts => posts.userId == item.userId).ToList();
                }
            }

            ViewData["postListViewData"] = postList;

            return View();
        }

        //[HttpGet]
        //[Route("/Post/Index")]
        //public async Task<ActionResult> Index(Post? item)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            string apiUrl = "https://jsonplaceholder.typicode.com/posts";
        //            HttpResponseMessage response = await client.GetAsync(apiUrl);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                string responseBody = await response.Content.ReadAsStringAsync();
        //                List<Post> postList = JsonConvert.DeserializeObject<List<Post>>(responseBody)!;

        //                if (item != null)
        //                {
        //                    if (item.id != 0)
        //                    {
        //                        postList = postList.Where(posts => posts.id == item.id).ToList();
        //                    }
        //                    if (item.title != null && item.title != "")
        //                    {
        //                        postList = postList.Where(posts => posts.title.Contains(item.title)).ToList();
        //                    }
        //                    if (item.body != null && item.body != "")
        //                    {
        //                        postList = postList.Where(posts => posts.body.Contains(item.body)).ToList();
        //                    }
        //                    if (item.userId != 0)
        //                    {
        //                        postList = postList.Where(posts => posts.userId == item.userId).ToList();
        //                    }
        //                }

        //                ViewData["postListViewData"] = postList;

        //                return View();
        //            }
        //            else 
        //            {
        //                return Content($"Error: {response.StatusCode}");
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            return Content($"Error: {ex.Message}");
        //        }
        //    }
        //}

        [HttpPost]
        [Route("/Post/Index")]
        public async Task<ActionResult> Index(Post item, int? id)
        {
            List<Post> postList = await getPostList();
            ViewData["postListViewData"] = postList;
            return RedirectToAction("Index",item);
            //return View(item);
        }

        [HttpGet]
        [Route("/Post/Create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/Post/Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Post item)
        {
            using (HttpClient client = new HttpClient())
            {
                try 
                {
                    string apiUrl = "https://jsonplaceholder.typicode.com/posts";

                    string jsonItem = JsonConvert.SerializeObject(item);

                    HttpContent content = new StringContent(jsonItem, System.Text.Encoding.UTF8, "application/json"); 

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        //Post post = JsonConvert.DeserializeObject<Post>(responseBody)!;

                        //return RedirectToAction("Index");
                        return Content(responseBody);
                    }
                    else
                    {
                        return Content($"Error: {response.StatusCode}");
                    }

                }
                catch (Exception ex)
                {
                    return Content($"Error: {ex.Message}");
                }
            }
        }

        [HttpGet]
        [Route("/Post/Edit")]
        public ActionResult Edit(Post item)
        {
            return View(item);
        }

        [HttpPost]
        [Route("/Post/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Post item, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = "https://jsonplaceholder.typicode.com/posts/1";

                    string updatedItem = JsonConvert.SerializeObject(item);

                    HttpContent content = new StringContent(updatedItem, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Post itemResult = JsonConvert.DeserializeObject<Post>(responseBody)!;
                        return Content(responseBody);
                    }
                    else
                    {
                        return Content($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    return Content($"Error: {ex.Message}");
                }
            }
        }

        //[HttpPost]
        //[Route("/Post/Edit")]
        //public async Task<ActionResult> Edit(Post item)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            string apiUrl = "https://jsonplaceholder.typicode.com/posts/1";

        //            string itemJson = JsonConvert.SerializeObject(item);

        //            HttpContent content = new StringContent(itemJson, System.Text.Encoding.UTF8, "application/json");

        //            HttpResponseMessage response = await client.PutAsync(apiUrl, content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                string responseBody = await response.Content.ReadAsStringAsync();
        //                Post responsePost = JsonConvert.DeserializeObject<Post>(responseBody)!;

        //                return Content(responseBody);
        //            }
        //            else
        //            {
        //                return Content($"Error: {response.StatusCode}");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Content($"Error: {ex.Message}");
        //        }
        //    }
        //}


        public async Task<List<Post>> getPostList()
        {
            using (HttpClient client = new HttpClient())
            {
                List<Post> postList = new List<Post>();

                try
                {
                    string apiUrl = "https://jsonplaceholder.typicode.com/posts";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    postList = JsonConvert.DeserializeObject<List<Post>>(responseBody)!;
                    return postList;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return postList;
            }
        }

}
}
