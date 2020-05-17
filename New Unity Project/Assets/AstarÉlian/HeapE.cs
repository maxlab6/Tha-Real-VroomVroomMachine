using System; 
using UnityEngine; 
 
//Classe de l'arbre organisationel qui permet de mieux organiser les sommets. 
public class HeapE<T> where T : IHeapItem<T> 
{ 
    T[] items;              //Array de tout ce qui est dans l'arbre. 
    int currentItemCount;   //Nombre de chose dans l'arbre. 
 
    //Fonction constructeur d'un objet arbre. 
    public HeapE(int maxHeapSize) 
    { 
        items = new T[maxHeapSize]; 
    } 
 
    //Fonction qui ajoute un item à l'arbre en gardant en mémoire l'endroit ou l'item se trouve. 
    public void Add(T item) 
    { 
        item.HeapIndex = currentItemCount; 
        items[currentItemCount] = item; 
        SortUp(item); 
        currentItemCount++; 
    } 
 
    //Fonction qui enlève le premier item de l'arbre. 
    public T RemoveFirst() 
    { 
        T firstItem = items[0]; 
        currentItemCount--; 
        items[0] = items[currentItemCount]; 
        items[0].HeapIndex = 0; 
        SortDown(items[0]); 
        return firstItem; 
    } 
 
    //Fonction qui classe les items. 
    public void UpdateItem(T item) 
    { 
        SortUp(item); 
    } 
 
    //Fonction qui retourne le nombre d'items dans l'arbre. 
    public int Count 
    { 
        get 
        { 
            return currentItemCount; 
        } 
    } 
 
    //Fonction qui cherche un item dans l'arbre. 
    public bool Contains(T item) 
    { 
        return Equals(items[item.HeapIndex], item); 
    } 
 
    //Fonction qui classe chaque item de l'arbre de façon inversée. 
    void SortDown(T item) 
    { 
        while (true) 
        { 
            int childIndexLeft = item.HeapIndex * 2 + 1; 
            int childIndexRight = item.HeapIndex * 2 + 2; 
            int swapIndex = 0; 
 
            if (childIndexLeft < currentItemCount) 
            { 
                swapIndex = childIndexLeft; 
 
                if (childIndexRight < currentItemCount) 
                { 
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) 
                    { 
                        swapIndex = childIndexRight; 
                    } 
                } 
 
                if (item.CompareTo(items[swapIndex]) < 0) 
                { 
                    Swap(item, items[swapIndex]); 
                } 
                else 
                { 
                    return; 
                } 
 
            } 
            else 
            { 
                return; 
            } 
 
        } 
    } 
 
    //Fonction qui classe chaque items de l'arbre. 
    void SortUp(T item) 
    { 
        int parentIndex = (item.HeapIndex - 1) / 2; 
 
        while (true) 
        { 
            T parentItem = items[parentIndex]; 
            if(item.CompareTo(parentItem) > 0) 
            { 
                Swap(item, parentItem); 
            } 
            else 
            { 
                break; 
            } 
            parentIndex = (item.HeapIndex - 1) / 2; 
        } 
    } 
 
    //Fonction qui échange de position deux items dans l'arbre. 
    void Swap(T itemA, T itemB) 
    { 
        items[itemA.HeapIndex] = itemB; 
        items[itemB.HeapIndex] = itemA; 
        int itemAIndex = itemA.HeapIndex; 
        itemA.HeapIndex = itemB.HeapIndex; 
        itemB.HeapIndex = itemAIndex; 
    } 
 
} 
 
 
public interface IHeapItem<T> : IComparable<T> 
{ 
    int HeapIndex 
    {
        get;
        set;
    }
}
