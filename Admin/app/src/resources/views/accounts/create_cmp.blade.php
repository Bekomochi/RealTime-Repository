<div>
    <!-- You must be the change you wish to see in the world. - Mahatma Gandhi -->
</div>

<h1>登録しました。</h1>

<form method="get" action="{{route('accounts.index')}}">
    @csrf
    <div class="d-flex justify-content-center">
        <input class="btn btn-secondary" type="submit" value="戻る">
    </div>
</form>
