using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [SerializeField] GameObject sheep;
    [SerializeField] GameObject pig;
    [SerializeField] GameObject cow;
    [SerializeField] Transform ProjectileTransform;

    private GameObject animal;
    private Rigidbody animalRig;
    private bool makeAim;

    private bool isMyTurn;
    public bool IsMyTurn { get { return isMyTurn; } }

    private bool animalSpent;
    public bool AnimalSpent { get { return animalSpent; } }

    public void TakeTurn(Projectile projectile)
    {
        projectile.GetComponent<Rigidbody>().velocity = new Vector3();
        projectile.transform.position = ProjectileTransform.position;
        //projectile.HasFinished = false;
        projectile.gameObject.SetActive(true);

        isMyTurn = true;
        animalSpent = false;
    }

    public void GiveTurn()
    {
        isMyTurn = false;
    }

    //todo иногда наблюдается баг - животное исчезает. И кнопки тоже задизейблены при этом)
    //todo сделать нормальный фон за пределами игрового поля
    //TODO животных отрисовать в шлемах, со щитами)
    //TODO и тут я придумал интереснейшую механику! В начале хода с вражеской стороны на игрока прет пехота! Он может раскуячить катапульту сразу, 
    //      но если пехота успеет добраться до него, то она раскуячит его, поэтому ее тоже можно и нужно куячить) Вот тут уже можно и уровней с возрастающей сложностью добавить
    //      и мультиплей с возможностью траты игровой валюты на пехоту и аптечки. И возможность покупки этой самой валюты)
    //      в качестве пехоты можно цаплей задействовать, атакующих клювом)
    //  Но в таком случае это уже либо не пошаговая игра будет, либо пехота должна останавливаться, когда ход переходит
    //TODO вместо PlayerWin текста показывать целое окно, в котором будет возможность рестарта или возврата в меню. 
    //      Окно будет показываться при выигрыше, нажатии на Esc или клику на красивую кнопочку с крестиком
    //TODO баг: если несброшенным животным навести на катапульту, то они взаимодействуют
    //TODO если игрок промахнулся животным, то ход переходит следующему
    //TODO при смерти катапульты - частицы с черепами
    //TODO рефакторинг - сделать фабрику животных. Все равно только 1 экземпляр в единицу времени, так пусть и список префабов будет у одного объекта
    //TODO выход из игры (меню при нажатии на Esc и при победе одного из игроков)
    //TODO добавить ветер и учесть его при настройке AI. Возможность отключить ветер в нстройках
    //TODO добавить звуков
    //TODO добавить искуственный интеллект
    //TODO добавить настройки (допустим, сложность и выключение звуков) (смотри в сторону PlayerPrefs или как их там)
    //TODO на высоких скоростях животное стремно сталкивается с катапультой - то ли сквозь проходит, то ли еще что
    //TODO баг - при высоком сбросе игрок сам в себя попадает (при FixedTime 0.0067 вроде норм все)
    //TODO это вообще лишнее) Добавить возможность указать имена игроков (если не сложно, то у компа оно будет таким до смены на другое (иначе Player2) - типа, вбил имя какого-нибудь мудака, ха-ха)
    //TODO если здоровье закончилось, то объявляем победителя (имя_игрока Win!)
    //TODO возможность, чтобы камера до определенной границы могла скрольнуть вверх мышкой (а потом и обратно, если передумали), чтобы можно было с большей высоты сбросить зверя
    //  в таком случае она должна потом и вниз проследовать за падающим зверем (или тупо скролить колесиком/тачскрином для мобилки)

    //TODO Советы от Юльки: обучалка, можно жизни показывать (не понятен смысл происхоящего)
    //  TODO Разрушающиеся стены перед катапультами

    //TODO по поводу искуственного интеллекта: комп делает пристрелочный сброс в предполагаемой точке с предплагаемой высоты, потом тупо меняет высоту, если не помогло - точку
    //  нужен список расстояний и точек м.б. с высотами (игра компов друг с другом и запись значений при попадании в файл). Расстояния между игроками можно самому выставлять
    //  потом в коде тупо брать нужную точку, а если нет, то брать два ближайших расстояния, между которыми текущее и уже приближать значения к текущей точке
    //в случае добавления искуственного интеллекта тупо сделать вторую сцену, где второй игрок - комп. В остальном все также. Унаследоваться от игрока по возможности
    //easy AI приближается к точке сброса, но даже приблизившись глушиться рандомом из большого промежутка, средний - рандомом из 3-5 значений. Тяжелый вообще не юзает рандом))

    //TODO сделать версию для андроида)
    //TODO а в версии для андроида сделать возможность покупать реактивные снаряды/прицельные и т.д. Ну и животных разных. И катапульту более красивую чтоб могли купить
    void Update()
    {
        if (!isMyTurn) return;

        if (makeAim)
        {
            SetAnimalTransform();

            if (Input.GetMouseButtonDown(0))
            {
                animalRig.isKinematic = false;
                makeAim = false;
            }
        }
    }

    public void CreateSheep()
    {
        CreateAnimal(sheep);
    }

    public void CreatePig()
    {
        CreateAnimal(pig);
    }

    public void CreateCow()
    {
        CreateAnimal(cow);
    }

    private void CreateAnimal(GameObject createdAnimal)
    {
        if (!isMyTurn) return;
        if (animal && animal.activeSelf)
            animal.SetActive(false);
        animal = createdAnimal;
        SetAnimalTransform();
        animalRig = createdAnimal.GetComponent<Rigidbody>();
        animalRig.isKinematic = true;
        createdAnimal.SetActive(true);
        makeAim = true;
        animalSpent = true;
    }

    void SetAnimalTransform()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        animal.transform.position = pos;
        animal.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
