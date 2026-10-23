public interface Output
{
	//接口里定义的变量是常量
	int MAX_CACHE_LINE = 50;
	//接口里定义的只能是public的抽象实例方法
	void out();
	void getData(String msg);
}

