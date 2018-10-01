# HttpService2Demo
网关核心服务2.0演示版

网关这块是整个微服务重要的核心，这块代码使用上基本没有任何限制，喜欢的朋友尽情使用就可以了。



这个版本我会移除相关的登录验证和缓存代码。主要目的是为了方便入门。



底层代码除了依赖.NET4.5，可以不依赖任何其他的组件。
数据库暂时使用SQLSERVER，使用的目的为了让人更好的理解系统的作用，毕竟日志可以可视化。
在生产环境下,缓存层使用了redis。 

主要功能：
对接口的标准进行了定义，并高性能访问其他对内HTTP服务。(不论定义还是返回，后续在补充)


是不是非常的简单，对的，核心层不需要太多的东西。
类似限流，熔断，日志，这些功能其实都是很容易拓展的，甚至可以用来实现的语言也特别多。
例如：日志的保存可以通过一些bat脚本来扩展。



需要看相关代码的可以看git删除代码的历史记录。
当然还有个2016年做的httpservice项目，喜欢的可以随意看看。
那个版本和现在的版本其实核心思想已经相差很大，除了http访问以外其他的基本算是从头来过。






