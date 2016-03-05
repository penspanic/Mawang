using System.Collections;

using System.Collections.Generic;

public class ObjectPool<T> where T : class
{

    // Instance count to create.  
    short count;
    public delegate T Func(T original);
    Func create_fn;

    // Instances.  
    Stack<T> objects;
    T original_object;


    // Construct  
    public ObjectPool(short count, T original_object, Func fn)
    {
        this.count = count;
        this.create_fn = fn;
        this.original_object = original_object;
        this.objects = new Stack<T>(this.count);


        allocate();
    }



    void allocate()
    {
        for (int i = 0; i < this.count; ++i)
        {
            this.objects.Push(this.create_fn(this.original_object));
        }
    }



    public T pop()
    {
        if (this.objects.Count <= 0)
        {
            allocate();
        }

        return this.objects.Pop();
    }



    public void push(T obj)
    {
        this.objects.Push(obj);
    }

}