# Sunshine.WebApi
common exception handlers and response wrapper for asp.net web api .

#background

I need a common exception handler for my asp.net api.
I need a response wrapper as the protocol between my api and client.
No changes should be make in my exists api actions of controllers.

I like the process that thinking about a architecture for common solutions

#protocol

When clients invoke a get/post api that returns signle object, api will wrap the result like 

{code:0,msg:'ok',data:'hello world'}

When clients invoke a get/post api that returns multi object, api will wrap the results like

{code:0,msg:'ok',data:['hello','world']}

When clients infoke a get/post api that returns exceptions , api will auto handle it using the default exception handler,
or the one that you implemented inherits from a base class provided by this project simply return a code and a msg.


{code:500,msg='default msg or you defined.'} 

The code value and msg content is depend on your implementation if you want.

Also it handle Model Validation Errors and Unauthorized Request using the same style.

Each of these handlers are have a default implementation.

by default 

0 represent no errors 
500 represent exception
7001 represent validation error

# usage

I provide a action filter for common use, it includes the handlers said above, it is /Infrastructure/ApiResultFilterAttribute.cs
In many case, you should create your own filter like what i did in ApiResultFilterAttribute.

when use, register it as global filter in webapi config cs file, or just put it together with a controller or action.

#paging data

It includes a lib 'MvcPager' by webdiyer (http://en.webdiyer.com/mvcpager/) to supports list paging.
I think many  apis for mobile app needs it.

When the api returns data with type PagedList<T>, contex handler will response like 

{code:0,msg='ok',hasMore:ture,data[]} 

Pageindex and pagesize parameter should be use in request but not shows at response.a 'hasMore' parameter instead.











