using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseTable<T>
{
    void Init();
    void Create(T configuration);

}
